using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Arction;

namespace ElementHostGaugeTest
{
    /// <summary>
    /// Interaction logic for Gauge.xaml
    /// </summary>
    public partial class MDACCustomGaugeControl : UserControl
    {
        private GaugeModelView mGaugeModelView;

        public MDACCustomGaugeControl(string PrimaryScaleText, double GaugeMinVal, double GaugeMaxVal, double GaugeAngleStart, double GaugeAngleEnd, double GaugeNormalAngleStart, double GaugeNormalAngleEnd, double GaugeAlertAngleStart, double GaugeAlertAngleEnd, double GaugeAlarmAngleStart, double GaugeAlarmAngleEnd)
        {
            InitializeComponent();
            mGaugeModelView = GaugeModelView.GaugeModelViewNewGauge(PrimaryScaleText, GaugeMinVal, GaugeMaxVal, GaugeAngleStart, GaugeAngleEnd, GaugeNormalAngleStart, GaugeNormalAngleEnd, GaugeAlertAngleStart, GaugeAlertAngleEnd, GaugeAlarmAngleStart, GaugeAlarmAngleEnd);
            base.DataContext = mGaugeModelView;
        }

        public void SetDataValue(double value)
        {
            mGaugeModelView.GaugeData = value;
        }
    }
}
