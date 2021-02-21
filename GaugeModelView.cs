/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Gauge Model View                                       */
/* DESCRIPTION:                                                               */
/* FILE NAME:          GaugeModelView.cs                                      */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementHostGaugeTest
{
    public class GaugeModelView : INotifyPropertyChanged
    {
        private double _GaugeValue;
        private string _GaugeText;
        private double _GaugeMinVal;
        private double _GaugeMaxVal;
        private double _GaugeAngleStart;
        private double _GaugeAngleEnd;
        private double _GaugeNormalAngleStart;
        private double _GaugeNormalAngleEnd;
        private double _GaugeAlertAngleStart;
        private double _GaugeAlertAngleEnd;
        private double _GaugeAlarmAngleStart;
        private double _GaugeAlarmAngleEnd;
        private double _GaugeMajorTickCount;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private GaugeModelView(string PrimaryScaleText, double GaugeMinVal, double GaugeMaxVal, double GaugeAngleStart, double GaugeAngleEnd, double GaugeNormalAngleStart, double GaugeNormalAngleEnd, double GaugeAlertAngleStart, double GaugeAlertAngleEnd, double GaugeAlarmAngleStart, double GaugeAlarmAngleEnd)
        {
            _GaugeValue = 0;
            _GaugeText = PrimaryScaleText;
            _GaugeMinVal = GaugeMinVal;
            _GaugeMaxVal = GaugeMaxVal;
            _GaugeAngleStart = GaugeAngleStart;
            _GaugeAngleEnd = GaugeAngleEnd;
            _GaugeNormalAngleStart = GaugeNormalAngleStart;
            _GaugeNormalAngleEnd = GaugeNormalAngleEnd;
            _GaugeAlertAngleStart = GaugeAlertAngleStart;
            _GaugeAlertAngleEnd = GaugeAlertAngleEnd;
            _GaugeAlarmAngleStart = GaugeAlarmAngleStart;
            _GaugeAlarmAngleEnd = GaugeAlarmAngleEnd;

            double ScaleGranularity = 1;

            if (0 == ((int)GaugeMinVal % 0.1) & (0 == ((int)GaugeMaxVal % 0.1))) { ScaleGranularity = 0.1; }
            if (0 == ((int)GaugeMinVal % 0.5) & (0 == ((int)GaugeMaxVal % 0.5))) { ScaleGranularity = 0.5; }
            if (0 == ((int)GaugeMinVal % 1) & (0 == ((int)GaugeMaxVal % 1))) { ScaleGranularity = 1; }
            if (0 == ((int)GaugeMinVal % 5) & (0 == ((int)GaugeMaxVal % 5))) { ScaleGranularity = 5; }
            if (0 == ((int)GaugeMinVal % 10) & (0 == ((int)GaugeMaxVal % 10))) { ScaleGranularity = 10; }
            if (0 == ((int)GaugeMinVal % 50) & (0 == ((int)GaugeMaxVal % 50))) { ScaleGranularity = 50; }
            if (0 == ((int)GaugeMinVal % 100) & (0 == ((int)GaugeMaxVal % 100))) { ScaleGranularity = 100; }
            if (0 == ((int)GaugeMinVal % 500) & (0 == ((int)GaugeMaxVal % 500))) { ScaleGranularity = 500; }
            if (0 == ((int)GaugeMinVal % 1000) & (0 == ((int)GaugeMaxVal % 1000))) { ScaleGranularity = 1000; }

            //int Grads = 1 + (((int)GaugeMaxVal - (int)GaugeMinVal) / (int)ScaleGranularity);
            int Grads = 11;
            _GaugeMajorTickCount = (double)Grads;
        }

        public static GaugeModelView GaugeModelViewNewGauge(string PrimaryScaleText, double GaugeMinVal, double GaugeMaxVal, double GaugeAngleStart, double GaugeAngleEnd, double GaugeNormalAngleStart, double GaugeNormalAngleEnd, double GaugeAlertAngleStart, double GaugeAlertAngleEnd, double GaugeAlarmAngleStart, double GaugeAlarmAngleEnd)
        {
            return new GaugeModelView(PrimaryScaleText, GaugeMinVal, GaugeMaxVal, GaugeAngleStart, GaugeAngleEnd, GaugeNormalAngleStart, GaugeNormalAngleEnd, GaugeAlertAngleStart, GaugeAlertAngleEnd, GaugeAlarmAngleStart, GaugeAlarmAngleEnd);
        }

        public double GaugeData
        {
            get { return this._GaugeValue; }
            set
            {
                this._GaugeValue = value;
                NotifyPropertyChanged("GaugeData");
            }
        }

        public string GaugeText
        {
            get { return this._GaugeText; }
            set
            {
                this._GaugeText = value;
                NotifyPropertyChanged("GaugeText");
            }
        }

        public double GaugeMinVal
        {
            get { return this._GaugeMinVal; }
            set
            {
                this._GaugeMinVal = value;
                NotifyPropertyChanged("GaugeMinVal");
            }
        }

        public double GaugeMaxVal
        {
            get { return this._GaugeMaxVal; }
            set
            {
                this._GaugeMaxVal = value;
                NotifyPropertyChanged("GaugeMaxVal");
            }
        }

        public double GaugeAngleStart
        {
            get { return this._GaugeAngleStart; }
            set
            {
                this._GaugeAngleStart = value;
                NotifyPropertyChanged("GaugeAngleStart");
            }
        }

        public double GaugeAngleEnd
        {
            get { return this._GaugeAngleEnd; }
            set
            {
                this._GaugeAngleEnd = value;
                NotifyPropertyChanged("GaugeAngleEnd");
            }
        }

        public double GaugeNormalAngleStart
        {
            get { return this._GaugeNormalAngleStart; }
            set
            {
                this._GaugeNormalAngleStart = value;
                NotifyPropertyChanged("GaugeNormalAngleStart");
            }
        }

        public double GaugeNormalAngleEnd
        {
            get { return this._GaugeNormalAngleEnd; }
            set
            {
                this._GaugeNormalAngleEnd = value;
                NotifyPropertyChanged("GaugeNormalAngleEnd");
            }
        }

        public double GaugeAlertAngleStart
        {
            get { return this._GaugeAlertAngleStart; }
            set
            {
                this._GaugeAlertAngleStart = value;
                NotifyPropertyChanged("GaugeAlertAngleStart");
            }
        }

        public double GaugeAlertAngleEnd
        {
            get { return this._GaugeAlertAngleEnd; }
            set
            {
                this._GaugeAlertAngleEnd = value;
                NotifyPropertyChanged("GaugeAlertAngleEnd");
            }
        }

        public double GaugeAlarmAngleStart
        {
            get { return this._GaugeAlarmAngleStart; }
            set
            {
                this._GaugeAlarmAngleStart = value;
                NotifyPropertyChanged("GaugeAlarmAngleStart");
            }
        }

        public double GaugeAlarmAngleEnd
        {
            get { return this._GaugeAlarmAngleEnd; }
            set
            {
                this._GaugeAlarmAngleEnd = value;
                NotifyPropertyChanged("GaugeAlarmAngleEnd");
            }
        }

        public double GaugeMajorTickCount
        {
            get { return this._GaugeMajorTickCount; }
            set
            {
                this._GaugeMajorTickCount = value;
                NotifyPropertyChanged("GaugeMajorTickCount");
            }
        }
    }
}
