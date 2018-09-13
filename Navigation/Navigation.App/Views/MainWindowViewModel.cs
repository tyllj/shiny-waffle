using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Navigation.App.Annotations;
using Navigation.App.Graphics;
using Navigation.App.Projections;
using Navigation.App.UI;
using Navigation.Entities;
using Navigation.Routing;
using Xamarin.Forms;
using Forms = Xamarin.Forms;

namespace Navigation.App.Views
{
    public class MainWindowViewModel : ViewModel
    {
        #region private fields

        private readonly IMapProvider _mapProvider;
        private readonly IRouter _router;
        private readonly IMapRenderer _mapRenderer;

        #endregion
        
        #region constructor
        
        public MainWindowViewModel( IMapProvider mapProvider,
                                    IRouter router,
                                    IMapRenderer mapRenderer)
        {
            _mapProvider = mapProvider;
            _router = router;
            _mapRenderer = mapRenderer;
            PlanTripCommand = new YaCommand(_ => PlanTrip());
        }
        
        #endregion

        #region properties
        
        public IEnumerable<Node> Places
        {
            get => _mapProvider.GetAllNodes();
        }
        
        public Node Start { get; set; }
        
        public Node Destination { get; set; }
        
        #endregion
        
        #region commands

        public ICommand PlanTripCommand { get; }

        #endregion
        
        #region public methods
        
        public Stream DrawMap()
        {
            var imageStream = _mapRenderer.Render();
            return imageStream;
        }

        #endregion
        
        #region non-public methods

        private void PlanTrip()
        {
            var route = _router.FindRoute(Start.Id, Destination.Id);
            _mapRenderer.HighlightedEdges.Clear();

            for (var i = 0; i < route.Count - 1; i++)
            {
                var highlightedEdge = new NodeToNodeEdge(route[i].Id, route[i + 1].Id);
                _mapRenderer.HighlightedEdges.Add(highlightedEdge);
            }

            MapChanged?.Invoke(this, EventArgs.Empty);
        }
        
        #endregion
        
        #region events

        public event EventHandler MapChanged;

        #endregion
    }
}