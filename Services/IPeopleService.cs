using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.DTO;

namespace SecretSanta.Services
{
    public interface IPeopleService
    {
        Task<PersonDTO> CreatePersonAsync(PersonCreateDTO dto);
    }
}