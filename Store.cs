using Microsoft.JSInterop;
using System.Text.Json;
namespace FlexDaily;
public class Store(IJSRuntime js){public async Task<List<Session>> Load()=>await js.InvokeAsync<List<Session>>("flexDb.load")??new();public Task Save(Session s)=>js.InvokeVoidAsync("flexDb.save",s).AsTask();public Task Clear()=>js.InvokeVoidAsync("flexDb.clear").AsTask();public Task Beep()=>js.InvokeVoidAsync("flexDb.beep").AsTask();public Task Download(string data)=>js.InvokeVoidAsync("flexDb.download",data).AsTask();}
public class Session{public string Id{get;set;}=Guid.NewGuid().ToString();public string Date{get;set;}=DateTime.Now.ToString("yyyy-MM-dd");public string Focus{get;set;}="hip";public int Seconds{get;set;}public int Before{get;set;}public int After{get;set;}public List<string> Exercises{get;set;}=new();}
public record Exercise(string Name,int Seconds,string Focus,string Phase,string Instructions,bool Kneeling=false,bool Floor=false);
public class Step{public Exercise Exercise{get;set;}=default!;public bool Done{get;set;}}
public static class Library{
public static readonly List<Exercise> All=new(){
new("Marching in place",60,"all","Warm-up","March gently, lifting each knee only as high as comfortable."),
new("Standing hip circles",60,"hip full back","Warm-up","Make slow small circles, switching direction halfway."),
new("Gentle leg swings",60,"hip legs full","Warm-up","Hold a stable support and swing each leg gently."),
new("Standing hip flexor stretch",120,"hip full","Stretch","Tuck pelvis and squeeze rear glute; switch legs every 30 seconds."),
new("Butterfly stretch",120,"hip legs full","Stretch","Sit with soles together; let knees relax outward.",false,true),
new("Adductor rock-backs",120,"hip legs full","Mobility","On hands and knees extend one leg sideways, rock hips backward; switch halfway.",true,true),
new("Standing quadriceps stretch",120,"hip legs full","Stretch","Hold wall for balance; gently draw heel toward buttock; alternate sides."),
new("90/90 hip rotations",120,"hip full","Mobility","Sit with knees bent and rotate legs side to side without forcing.",false,true),
new("Glute bridges",120,"hip back full","Strength","Lie on back, knees bent; squeeze glutes to lift hips, then lower.",false,true),
new("Single knee-to-chest",60,"hip back full","Cooldown","Lie on back; draw one knee toward chest; switch halfway.",false,true),
new("Cat-cow",60,"back full","Mobility","On hands and knees slowly round and extend spine.",true,true),
new("Hamstring stretch",60,"legs full","Stretch","Gently hinge toward extended leg, switch halfway.",false,true),
new("Standing side bends",60,"back full","Mobility","Stand tall and slowly lean side to side."),
new("Supported side lunge",90,"hip legs full","Mobility","Hold a stable support and gently shift weight from side to side."),
new("Standing glute squeeze",60,"hip full","Strength","Stand upright and gently squeeze glutes for three seconds, release.")};
public static List<Step> Generate(string focus,int minutes,bool noKneeling,bool noFloor){var pool=All.Where(x=>(!noKneeling||!x.Kneeling)&&(!noFloor||!x.Floor)).ToList();var selected=new List<Exercise>();void Add(Exercise? e){if(e!=null&&!selected.Contains(e))selected.Add(e);}var rng=Random.Shared;Exercise? Pick(IEnumerable<Exercise> xs)=>xs.OrderBy(_=>rng.Next()).FirstOrDefault();Add(Pick(pool.Where(x=>x.Phase=="Warm-up")));var relevant=pool.Where(x=>x.Focus.Split(' ').Contains(focus)||x.Focus=="all").ToList();Add(Pick(relevant.Where(x=>x.Phase=="Stretch")));Add(Pick(relevant.Where(x=>x.Phase=="Mobility")));Add(Pick(relevant.Where(x=>x.Phase=="Strength")));int total=selected.Sum(x=>x.Seconds);foreach(var e in relevant.Concat(pool).Distinct().OrderBy(_=>rng.Next())){if(total>=minutes*60-30)break;if(!selected.Contains(e)&&total+e.Seconds<=minutes*60+30){Add(e);total+=e.Seconds;}}return selected.Select(x=>new Step{Exercise=x}).ToList();}
}
