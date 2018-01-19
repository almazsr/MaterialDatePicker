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
        public readonly UIColor CalendarColor = new UIColor(red: 0.10f, green: 0.47f, blue: 0.73f, alpha: 1.0f);

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
            dialog.Header.LabelDayName.BackgroundColor = CalendarColor;
            dialog.Header.BackgroundColor = CalendarColor;

            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(0)] = CalendarColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(1)] = CalendarColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(2)] = CalendarColor;
            dialog.Calendar.BackgroundColors[NSNumber.FromInt16(4)] = CalendarColor;

            dialog.ButtonCancel.SetTitle("ОТМЕНА", UIControlState.Normal);
            dialog.Calendar.TitleColors[NSNumber.FromInt16(8)] = CalendarColor;
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
