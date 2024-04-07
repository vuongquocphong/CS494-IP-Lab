using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Component;
using GameComponents;
using Godot;

namespace Mediator{
    
    public class EventPasser : IMediator 
    {
        // Singleton
        private static EventPasser _instance = null!;

        // Constant path name
        public const String INPUT_NAME_PANEL = "res://scenes/InputNamePanel.tscn";
        public const String WAITING_PANEL = "res://scenes/WaitingPanel.tscn";
        public const String INGAME_PANEL = "res://scenes/IngamePanel.tscn";
        public const String SCOREBOARD_PANEL = "res://scenes/ScoreboardPanel.tscn";
        
        // scripting components
        public InputNameComponent InputNameComp;
        public WaitingComponent WaitingComp;
        public IngameComponent IngameComp;
        public ScoreboardComponent ScoreboardComp;
        public GameManager GameManagerComp;
        private SceneTree Tree; // Use this to navigate among scenes
        private EventPasser(SceneTree Tree)
        {
            InputNameComp = new InputNameComponent(this);
            WaitingComp = new WaitingComponent(this);
            IngameComp = new IngameComponent(this);
            ScoreboardComp = new ScoreboardComponent(this);
            // GameManagerComp = new GameManager(this);
            this.Tree = Tree;
            Node FirstScene = ResourceLoader.Load<PackedScene>(INPUT_NAME_PANEL).Instantiate();
            this.Tree.CallDeferred(Window.MethodName.AddChild, FirstScene);
        }

        public static EventPasser GetInstance(SceneTree Tree)
        {
            if (_instance == null)
            {
                _instance = new EventPasser(Tree);
            }
            return _instance;
        }
        public void Notify(object sender, Event ev){
            switch (sender) 
            {
                case GameManager:
                    ReactOnGameManager(ev);
                    break;
                case InputNameComponent:
                    ReactOnInputNamePanel(ev);
                    break;
                case WaitingComponent:
                    ReactOnWaitingPanel(ev);
                    break;
                case IngameComponent:
                    ReactOnIngamePanel(ev);
                    break;
                case ScoreboardComponent:
                    ReactOnScoreboardPanel(ev);
                    break;
            }
        }

        private void ReactOnScoreboardPanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnIngamePanel(Event ev)
        {
            throw new NotImplementedException();
        }

        private void ReactOnWaitingPanel(Event ev)
        {
            switch(ev){
                case Event.DISCONNECT:
                    TransitionTo(INPUT_NAME_PANEL);
                    break;
            }
        }

        private void ReactOnInputNamePanel(Event ev)
        {
            switch(ev){
                case Event.REQUEST_CONNECT:
                    TransitionTo(WAITING_PANEL);
                    break;
            }
        }
        private void ReactOnGameManager(Event ev)
        {
            switch(ev) {
                // Input name scene
                case Event.CONNECT_SUCCESS:
                    
                case Event.CONNECT_FAILURE:
                    break;
            }
        }

        private void TransitionTo(String SceneName){
            Tree.ChangeSceneToFile(SceneName);
        }
    }
}