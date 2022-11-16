using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MVVMCalculator.ViewModels
{
    public class ViewModelCalculator : ViewModelBase
    {
        #region Propiedades
        int currentState = 1;
        string mathOperator;

        double firstNumber;
        public double FirstNumber
        {
            get { return firstNumber; }
            set
            {
                if (firstNumber != value)
                {
                    firstNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        double secondNumber;
        public double SecondNumber
        {
            get { return secondNumber; }
            set
            {
                if (secondNumber != value)
                {
                    secondNumber = value;
                    OnPropertyChanged();
                }
            }
        }


        string result;
		public string Result
		{
			get { return result; }
			set
			{
				if (result != value)
				{
					result = value;
					OnPropertyChanged();
				}
			}
		}

        #endregion
        public ICommand OnSelectOperator { protected set; get; }
        public ICommand OnCalculator { protected set; get; }
        public ICommand OnSelectNumber { protected set; get; }
        public ICommand OnClear { protected set; get; }

        

        public ViewModelCalculator()
        {
            OnClear = new Command(() =>
            {
                firstNumber = 0;
                secondNumber = 0;
                currentState = 1;
                this.Result = "0";
            });

            OnSelectNumber = new Command<string>(
               execute: (string parameter) =>
               {
				   
				   string pressed = parameter;

				   if (Result == "0" || currentState < 0)
				   {
					   Result = "";
					   if (currentState < 0)
						   currentState *= -1;
				   }

				   Result += pressed;

				   double number;
				   if (double.TryParse(Result, out number))
				   {
					   Result = number.ToString("N0");
					   if (currentState == 1)
					   {
						   firstNumber = number;
					   }
					   else
					   {
						   secondNumber = number;
					   }
				   }
			   });

            OnSelectOperator = new Command<string>(
                execute: (string parameter) =>
                {

                    currentState = -2;
                    string pressed = parameter;
                    mathOperator = pressed;
                });

            OnCalculator = new Command(() =>
            {
                if (currentState == 2)
                {
                    var result = SimpleCalculator.Calculate(firstNumber, secondNumber, mathOperator);

                    Result = result.ToString("N0");
                    firstNumber = result;
                    currentState = -1;
                }
            }
            );

        }


    }
}
