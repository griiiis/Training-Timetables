﻿using App.BLL.DTO.DTOs.Contests;
using App.BLL.DTO.Models;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IContestService : IEntityService<App.BLL.DTO.Contest>, IContestRepositoryCustom<App.BLL.DTO.Contest>
{ 
    App.BLL.DTO.Contest AddContestWithUser(Guid userId, App.BLL.DTO.Contest contest);
    bool IsContestOwnedByUser (Guid userId, Guid contestId);
    App.BLL.DTO.Contest UpdateContestWithUser(Guid userId, App.BLL.DTO.Contest contest);
    Task<EditContestDTO> GetContestEditModel(Guid userId, Guid id);
    Task PutContest(Guid userId, Guid id, EditContestDTO contestDTO);
    Task<App.BLL.DTO.Contest> PostContest(Guid userId, CreateContestDTO contest);
    Task<IEnumerable<OwnerContestsDTO>> GetOwnerContests(Guid userId);

}