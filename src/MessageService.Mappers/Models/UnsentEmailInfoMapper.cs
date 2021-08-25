﻿using LT.DigitalOffice.MessageService.Mappers.Models.Interfaces;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Models.Emails;
using System;

namespace LT.DigitalOffice.MessageService.Mappers.Models
{
    public class UnsentEmailInfoMapper : IUnsentEmailInfoMapper
    {
        private readonly IEmailInfoMapper _mapper;

        public UnsentEmailInfoMapper(
            IEmailInfoMapper mapper)
        {
            _mapper = mapper;
        }

        public UnsentEmailInfo Map(DbUnsentEmail email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return new UnsentEmailInfo
            {
                Id = email.Id,
                Email = _mapper.Map(email.Email),
                CreatedAtUtc = email.CreatedAtUtc,
                LastSendAtUtc = email.LastSendAtUtc,
                TotalSendingCount = email.TotalSendingCount
            };
        }
    }
}
