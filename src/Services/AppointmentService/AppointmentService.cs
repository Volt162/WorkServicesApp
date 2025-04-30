using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MopsterTeams.Models;
using MopsterTeams.Models.AppointmentModel;
using MopsterTeams.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using MopsterTeamsMaui;

namespace MopsterTeams.Services
{
    public class AppointmentService : BaseServiceWithAuthorization, IAppointmentService
    {
        private const int _downloadImageTimeoutInSeconds = 30;
        readonly HttpClient _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(_downloadImageTimeoutInSeconds) };

        public AppointmentService(IRestService rest,
             ISettingsService settingsManager,
             IInternetConnectionService internetConnectionService) : 
            base(rest, settingsManager, internetConnectionService)
        {
            
        }

        #region -- IAppointmentService implementation --

        public async Task<AOResult<SalesAppointment>> GetSalesAppointmentAsync(string id)
        {
            var res = new AOResult<SalesAppointment>();

            try
            {
                var serverResponse = await GetAsync<SalesAppointment>($"{SettingsService.BaseUrl}/api/xxx/GetSalesAppointmentForEmployeeApp?SalesAppointmentId={id}", null, null);

                if (serverResponse != null)
                {
                    res.SetSuccess(serverResponse);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("GetSalesAppointmentAsync_Exception", ex.Message, ex);
            }

            return res;
        }

        public async Task<AOResult<ServiceAppointment>> GetServiceAppointmentAsync(string id)
        {
            var res = new AOResult<ServiceAppointment>();

            try
            {
                var serverResponse = await GetAsync<ServiceAppointment>($"{SettingsService.BaseUrl}/api/xxx/GetServiceAppointmentForEmployeeApp?ServiceAppointmentId={id}", null, null);

                if (serverResponse != null)
                {
                    res.SetSuccess(serverResponse);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("GetServiceAppointmentAsync_Exception", ex.Message, ex);
            }

            return res;
        }

        public async Task<AOResult<byte[]>> GetServiceAppointmentAttachmentThumbnailAsync(string id)
        {
            var res = new AOResult<byte[]>();

            try
            {
                var serverResponse = await DownloadImageAsync($"{SettingsService.BaseUrl}/api/xxx/GetServiceAppointmentAttachmentThumbnail?attachmentId={id}");

                if (serverResponse.IsSuccess)
                {
                    res.SetSuccess(serverResponse.Result);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("GetServiceAppointmentAttachmentThumbnailAsync_Exception", ex.Message, ex);
            }

            return res;
        }


        public async Task<AOResult<byte[]>> GetSalesAppointmentAttachmentThumbnailAsync(string id)
        {
            var res = new AOResult<byte[]>();

            try
            {
                var serverResponse = await DownloadImageAsync($"{SettingsService.BaseUrl}/api/xxx/GetSalesAppointmentAttachmentThumbnail?attachmentId={id}");

                if (serverResponse.IsSuccess)
                {
                    res.SetSuccess(serverResponse.Result);
                }
                else
                {
                    res.SetFailure();
                }
            }
            catch (Exception ex)
            {
                res.SetError("GetSalesAppointmentAttachmentThumbnailAsync_Exception", ex.Message, ex);
            }

            return res;
        }

        #endregion

        #region -- Private helpers --

        public async Task<HttpResponseMessage> UploadImageAsync(IList<FileReportAttachmentViewModel> files, string url)
        {
            await RefreshTokenIfNeededAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            var headerParams = GetHeadersForAutorization();
            foreach (var headerParam in headerParams)
            {
                _httpClient.DefaultRequestHeaders.Add(headerParam.Key, headerParam.Value);
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            using (var formData = new MultipartFormDataContent())
            {
                foreach (var item in files)
                {
                    if (item.FileData == null)
                    {
                        continue;
                    }
                    var stream = new MemoryStream(item.FileData);
                    HttpContent fileStreamContent = new StreamContent(stream);

                    if (MimeTypes.TryGetMimeType(item.Name, out var mimeType))
                    {
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                        System.Diagnostics.Debug.WriteLine($"mimeType - {mimeType}");
                    }
                    else
                    {
                        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                        System.Diagnostics.Debug.WriteLine($"mimeType - multipart/form-data");
                    }
                    fileStreamContent.Headers.Add("Content-Length", stream.Length.ToString());

                    formData.Add(fileStreamContent, "\"files\"", $"\"{item.Name}\"");
                }
                var response = await _httpClient.PostAsync(url, formData);
                return response;
            }
        }

        private async Task<AOResult<byte[]>> DownloadImageAsync(string url)
        {
            var aoResult = new AOResult<byte[]>();

            await RefreshTokenIfNeededAsync();
            _httpClient.DefaultRequestHeaders.Clear();
            var headerParams = GetHeadersForAutorization();
            foreach (var headerParam in headerParams)
            {
                _httpClient.DefaultRequestHeaders.Add(headerParam.Key, headerParam.Value);
            }
            
            using (var httpResponse = await _httpClient.GetAsync(url))
            {
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var result = await httpResponse.Content?.ReadAsByteArrayAsync();
                    aoResult.SetSuccess(result);
                }
                else
                {
                    //Url is Invalid
                    aoResult.SetError("Invalid_Url", "Error when downloading image");
                }
            }

            return aoResult;
        }

        #endregion
    }
}