// using System.Data;
// using System.Runtime.CompilerServices;
// using System.Text.RegularExpressions;
// using GameComponents;
// using Godot;

// namespace Mediator{
    
//     public class EventPasser : IMediator 
//     {
//         // Singleton
//         public GameManager GameManager = null!;
//         public InputNamePanel inputNamePanel;
//         private EventPasser()
//         {
//             GameManager = GameManager.GetInstance(this);
//         }

//         public void Notify(Event ev){
            
//         }

//         private void ReactOnScoreboardPanel(Event ev)
//         {
//             throw new NotImplementedException();
//         }

//         private void ReactOnIngamePanel(Event ev)
//         {
//             throw new NotImplementedException();
//         }

//         private void ReactOnWaitingPanel(Event ev)
//         {
//             switch(ev){
//                 case Event.DISCONNECT:
//                     break;
//             }
//         }
//         private void ReactOnInputNamePanel(Event ev)
//         {
//             switch(ev){
//                 case Event.REQUEST_CONNECT:
//                     break;
//             }
//         }
//         private void ReactOnGameManager(Event ev)
//         {
//             switch(ev) {
//                 // Input name scene
//                 case Event.CONNECT_SUCCESS:
                    
//                 case Event.CONNECT_FAILURE:
//                     break;
//             }
//         }
//     }
// }