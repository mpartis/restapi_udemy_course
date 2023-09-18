﻿using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        // epistrefei Task giati theloume na einai async
        Task<Villa> UpdateAsync(Villa entity);

    }
}
