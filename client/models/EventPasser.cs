using System.Data;
using System.Text.RegularExpressions;
using GameComponents;
using Godot;

namespace Mediator{
    public class EventPasser : IMediator 
    {
        // scripting components
        private GameManager GameManagerComp;
        private InputNamePanel InputNamePanelComp;
        private WaitingPanel WaitingPanelComp;
        private IngamePanel IngamePanelComp;
        private ScoreboardPanel ScoreboardPanelComp;

        // Resource loaders
        private Node InputNameNode;
        private Node WaitingNode;
        private Node IngameNode;
        private Node ScoreboardNode;
        private Node CurrentScene;
        private SceneTree Tree; // Use this to navigate among scenes
        public EventPasser(SceneTree tree)
        {
            // Initialize resource loaders
            
            InputNameNode = tree.CurrentScene;
            WaitingNode = ResourceLoader.Load<PackedScene>("res://WaitingPanel.tscn").Instantiate();
            IngameNode = ResourceLoader.Load<PackedScene>("res://IngamePanel.tscn").Instantiate();
            ScoreboardNode = ResourceLoader.Load<PackedScene>("res://ScoreboardPanel.tscn").Instantiate();
            
            // Get components
            InputNamePanelComp = InputNameNode as InputNamePanel;
            WaitingPanelComp = WaitingNode as WaitingPanel;
            IngamePanelComp = IngameNode as IngamePanel;
            ScoreboardPanelComp = ScoreboardNode as ScoreboardPanel;
            GameManagerComp = new GameManager();
            
            // Set mediator
            InputNamePanelComp.SetMediator(this);
            WaitingPanelComp.SetMediator(this);
            this.Tree = tree;
            CurrentScene = Tree.CurrentScene;
        }

        public void Notify(object sender, Event ev){
            switch (sender) 
            {
                case GameManager:
                    ReactOnGameManager(ev);
                    break;
                case InputNamePanel:
                    ReactOnInputNamePanel(ev);
                    break;
                case WaitingPanel:
                    ReactOnWaitingPanel(ev);
                    break;
                case IngamePanel:
                    ReactOnIngamePanel(ev);
                    break;
                case ScoreboardPanel:
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
                    TransitionTo(InputNameNode);
                    break;
            }
        }

        private void ReactOnInputNamePanel(Event ev)
        {
            switch(ev){
                case Event.REQUEST_CONNECT:
                    TransitionTo(WaitingNode);
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

        private void TransitionTo(Node scene){
            Tree.Root.RemoveChild(CurrentScene);
            CurrentScene = scene;
            Tree.Root.AddChild(scene);
        }
    }
}