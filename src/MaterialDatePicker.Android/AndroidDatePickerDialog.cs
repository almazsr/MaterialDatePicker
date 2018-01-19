using System;
using System.Threading.Tasks;
using Android.App;
using ADatePickerDialog = Android.App.DatePickerDialog;

namespace MaterialDatePicker.Android
{
    public class AndroidDatePickerDialog : Java.Lang.Object, IDatePickerDialog
    {
        private static readonly DateTime UnixMinDate = new DateTime(1970, 1, 1);

        private readonly Activity _activity;

        private ADatePickerDialog _dialog;
        private TaskCompletionSource<DateTime?> _dateSetTcs;

        public AndroidDatePickerDialog(Activity activity)
        {
            _activity = activity;
        }

        public Task<DateTime?> PickDateAsync(DateTime? selectedDate = null, DateTime? minDate = null)
        {
            var date = selectedDate ?? DateTime.Today;
            _dialog = new ADatePickerDialog(_activity, OnDateSet, date.Year, date.Month - 1, date.Day);
            _dialog.CancelEvent += OnCanceled;
            if (minDate.HasValue)
            {
                _dialog.DatePicker.MinDate = (long)(minDate.Value - UnixMinDate).TotalSeconds;
            }
            _dateSetTcs = new TaskCompletionSource<DateTime?>();
            _dialog.Show();
            return _dateSetTcs.Task;
        }

        private void OnDateSet(object sender, ADatePickerDialog.DateSetEventArgs e)
        {
            _dateSetTcs.TrySetResult(e.Date);
            _dialog.CancelEvent -= OnCanceled;
            _dialog.DateSet -= OnDateSet;
        }

        private void OnCanceled(object sender, EventArgs e)
        {
            _dateSetTcs.TrySetResult(null);
            _dialog.CancelEvent -= OnCanceled;
            _dialog.DateSet -= OnDateSet;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dialog != null)
                {
                    _dialog.Dispose();
                    _dialog = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
