using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MopsterTeams.Models;
using MopsterTeams.ViewModels;

namespace MopsterTeams.Services
{
    public interface IAppointmentService
    {
        Task<AOResult<SalesAppointment>> GetSalesAppointmentAsync(string id);
        Task<AOResult<ServiceAppointment>> GetServiceAppointmentAsync(string id);
        Task<AOResult<byte[]>> GetServiceAppointmentAttachmentThumbnailAsync(string id);
        Task<AOResult<byte[]>> GetSalesAppointmentAttachmentThumbnailAsync(string id);
    }
}
