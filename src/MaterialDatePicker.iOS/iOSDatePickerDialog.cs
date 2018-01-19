using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace MaterialDatePicker.iOS
{
    public class iOSDatePickerDialog : NSObject, IDatePickerDialog
    {
        public UIColor AccentColor { get; set; }

        private MDDatePickerDialog _dialog;

        public class Delegate : MDDatePickerDialogDelegate
        {
            private readonly Action<DateTime> _action;

            public Delegate(Action<DateTime> action)
            {
                _action = action;
            }

            public override void DatePickerDialogDidSelectDate(NSDate date)
            {
                _action(date.ToDateTime().ToLocalTime());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dialog != null)
                {
                    _dialog.Dispose();
                    _dialog.Delegate = null;
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }

        protected virtual void Customize(MDDatePickerDialog dialog)
        {
            dialog.Header.LabelDayName.BackgroundColor = AccentColor;
            dialog.Header.BackgroundColor = AccentColor;

            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(0)] = AccentColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(1)] = AccentColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(2)] = AccentColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(4)] = AccentColor;
            
            dialog.Calendar.TitleColors[NSNumber.FromInt16(8)] = AccentColor;
        }

        public Task<DateTime?> PickDateAsync(DateTime? selectedDate = null, DateTime? minDate = null)
        {
            var date = selectedDate ?? DateTime.Today;
            var tcs = new TaskCompletionSource<DateTime?>();

            _dialog = new MDDatePickerDialog()
            {
                Delegate = new Delegate(d =>
                {
                    tcs.TrySetResult(d);
                }),
                SelectedDate = date.ToNSDate()
            };

            if (minDate.HasValue)
            {
                _dialog.MinimumDate = minDate.Value.ToNSDate();
            }

            Customize(_dialog);

            _dialog.Show();
            return tcs.Task;
        }
    }
}
