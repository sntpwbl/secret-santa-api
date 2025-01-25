using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretSanta.DTO;

namespace SecretSanta.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<PersonDTO>> GetAllPeopleAsync();
        Task<PersonDTO> GetSelectedPersonAsync(int personId);
        Task<PersonDTO> LoginAsync(PersonCreateDTO dto);
        Task<PersonDTO> CreatePersonAsync(PersonCreateDTO dto);
    }
}