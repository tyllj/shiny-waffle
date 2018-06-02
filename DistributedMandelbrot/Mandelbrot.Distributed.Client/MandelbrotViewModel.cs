using System.Windows.Input;
using Mandelbrot.Common.Gui;

namespace Mandelbrot.Distributed.Client
{
    public class MandelbrotViewModel
    {
        #region Private Fields

        

        #endregion
        
        #region Constructors

        public MandelbrotViewModel()
        {
            RenderCommand = new YaCommand(null);
        }

        #endregion
        
        #region Public Properties

        

        #endregion
        
        #region Commands
        
        public ICommand RenderCommand { get; }
        
        #endregion

        #region Public Methods

        

        #endregion

        #region Non-Public Methods

        

        #endregion
    }
}