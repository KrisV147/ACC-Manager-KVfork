﻿using RaceElement.HUD.Overlay.Internal;
using RaceElement.HUD.Overlay.Configuration;
using System.Drawing;


namespace RaceElement.HUD.ACC.Overlays.OverlayInputTrace
{
    [Overlay(Name = "Input Trace", Version = 1.00, OverlayType = OverlayType.Release,
        Description = "Live graph of steering, throttle and brake inputs.")]
    internal sealed class InputTraceOverlay : AbstractOverlay
    {
        private readonly InputTraceConfig _config = new InputTraceConfig();
        internal class InputTraceConfig : OverlayConfiguration
        {
            [ConfigGrouping("Chart", "Customize the charts refresh rate, data points or hide the steering input.")]
            public ChartGrouping InfoPanel { get; set; } = new ChartGrouping();
            public class ChartGrouping
            {
                [ToolTip("The amount of datapoints shown, this changes the width of the overlay.")]
                [IntRange(150, 800, 10)]
                public int DataPoints { get; set; } = 300;

                [ToolTip("Sets the data collection rate, this does affect cpu usage at higher values.")]
                [IntRange(10, 70, 5)]
                public int Herz { get; set; } = 30;

                [ToolTip("Displays the steering input as a white line in the trace.")]
                public bool SteeringInput { get; set; } = true;
            }

            public InputTraceConfig()
            {
                this.AllowRescale = true;
            }
        }

        private readonly int _originalHeight = 120;
        private readonly int _originalWidth = 300;

        private InputGraph _graph;
        private InputDataCollector _inputDataCollector;

        public InputTraceOverlay(Rectangle rectangle) : base(rectangle, "Input Trace")
        {
            _originalWidth = this._config.InfoPanel.DataPoints;
            this.Width = _originalWidth;
            this.Height = _originalHeight;
            this.RequestsDrawItself = true;
        }

        public sealed override void BeforeStart()
        {
            _inputDataCollector = new InputDataCollector(this) { TraceCount = this._originalWidth - 1, inputTraceConfig = _config };
            _inputDataCollector.Start();

            _graph = new InputGraph(0, 0, this._originalWidth - 1, this._originalHeight - 1, _inputDataCollector, this._config);
        }

        public sealed override void BeforeStop()
        {
            _inputDataCollector.Stop();
            _graph.Dispose();
        }

        public sealed override void Render(Graphics g)
        {
            _graph.Draw(g);
        }

        public sealed override bool ShouldRender()
        {
            return DefaultShouldRender();
        }
    }
}
