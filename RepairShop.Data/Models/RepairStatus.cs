using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RepairShop.Data.Models
{
    public enum RepairStatus
    {
        [Display(Name = "pending...")]
        Pending,
        [Display(Name = "In progress")]
        InProgress,
        [Display(Name = "Waiting for parts")]
        WaitingForComponents,
        Done
    }
}