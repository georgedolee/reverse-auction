using FileService.API.Interfaces;
using Grpc.Core;
using Photos;

namespace FileService.API.Services
{
    public class GrpcPhotoService : PhotoService.PhotoServiceBase
    {
        private readonly IPhotoStore _store;
        public GrpcPhotoService(IPhotoStore store)
        {
            _store = store;
        }

        public override async Task<UploadReply> Save(UploadRequest request, ServerCallContext context)
        {
            using var ms = new MemoryStream(request.Data.ToByteArray());
            var url = await _store.SaveAsync(ms, request.Filename, context.CancellationToken);
            return new UploadReply { Url = url };
        }

        public override Task<DeleteReply> Delete(DeleteRequest request, ServerCallContext context)
        {
            var success = _store.DeleteAsync(request.Url, context.CancellationToken);
            return Task.FromResult(new DeleteReply { Success = success });
        }
    }
}