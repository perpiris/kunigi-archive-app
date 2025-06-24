using KunigiArchive.Contracts.MediaFile;
using KunigiArchive.Web.ViewModels.Common;

namespace KunigiArchive.Web.Mappings;

public static class MediaFileMappings
{
    public static MediaFileViewModel MapToMediaFileViewModel(this MediaFileResponse response)
    {
        return new MediaFileViewModel
        {
            MediaFileId = response.MediaFileId,
            FileName = response.FileName,
            Path = response.Path
        };
    }
}