using System;
using System.Threading.Tasks;

namespace MaterialDatePicker
{
    public interface IDatePickerDialog
    {
        /// <summary>
        /// Picks date from dialog.
        /// </summary>
        /// <param name="selectedDate">Default selected date</param>
        /// <param name="minDate">Min date</param>
        /// <returns>Picked date from datepicker dialog. If no date picked and user closed the dialog, then returns null.</returns>
        Task<DateTime?> PickDateAsync(DateTime? selectedDate = null, DateTime? minDate = null);
    }
}
