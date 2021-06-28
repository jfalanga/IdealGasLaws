using System;
using System.Collections.Generic;
using System.Text;

namespace IdealGasLaws
{
    class IdealGass
    {
        private double _Mass;
        private double _Volume;
        private double _Temp;
        private double _Pressure;
        private double _MolecularWeight;

        public void SetMolecularWeight(double mw)
        {
            _MolecularWeight = mw;
            Calc();
        }

        public double GetMolecularWeight()
        {
            return _MolecularWeight;
        }
        public double GetPressure()
        {
            return _Pressure;
        }

        public double GetTemperature()
        {
            return _Temp;
        }

        public void SetTemperature(double temp)
        {
            _Temp = temp;
            Calc();
            
        }
        public void SetVolume(double vol)
        {
            _Volume = vol;
            Calc();
        }

        public double GetVolume()
        {
            return _Volume;
        }

        public void SetMass(double mass)
        {
            _Mass = mass;
            Calc();
        }

        public double GetMass()
        {
            return _Mass;
        }
        
        private void Calc()
        {
            //I need to remark that I did have some help on
            //this one- from my programmer father! Helped remind me of what
            //public, private, static, etc., mean!
            
            double n = Program.NumberOfMoles(GetMass(), GetMolecularWeight());
            double r = 8.3145;

            double temp = GetTemperature();
            temp=Program.CelsiusToKelvin(temp);

            _Pressure = ((n * r * temp) / GetVolume());
        }
        
    }
}
