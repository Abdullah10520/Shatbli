using MediatR;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetProductImageById
{
    public class GetCeramicQueryHandler : IRequestHandler<GetCeramicQuery, Result<GetCeramicResponse>>
    {
        private readonly IStorageService _storageService;

        public GetCeramicQueryHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }
        async Task<Result<GetCeramicResponse>> IRequestHandler<GetCeramicQuery, Result<GetCeramicResponse>>.Handle(GetCeramicQuery request, CancellationToken cancellationToken)
        {


            try
            {
                // 1. محاولة تحميل الـ Stream من خدمة التخزين
                var imageStream = await _storageService.downloadImageStream(request.CeramicId);

            // 2. التحقق مما إذا كان الـ Stream موجوداً
            if (imageStream == null)
            {
                // استخدام ميثود الـ NotFound الجاهزة في كلاس الـ Result الخاص بك
                return Result<GetCeramicResponse>.NotFound($"Ceramic image with ID {request.CeramicId} not found.");
            }

            // 3. بناء الرد في حالة النجاح
            GetCeramicResponse response = new GetCeramicResponse
                {
                    Stream = imageStream
                };

                return Result<GetCeramicResponse>.Success(response);
        }
            catch (Exception ex)
            {
                // هندلة أي خطأ غير متوقع (مثل فشل الاتصال بالـ Storage)
                return Result<GetCeramicResponse>.Failure(
                    message: "An error occurred while retrieving the ceramic image.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message}
                );
            }

            //var imageStream = await _storageService.downloadImageStream(request.CeramicId);
            //GetCeramicResponse response = new GetCeramicResponse();
            //response.Stream = imageStream;

            //return response;
        }
    }
}
