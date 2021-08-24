using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.MessageService.Data.Interfaces;
using LT.DigitalOffice.MessageService.Data.Provider;
using LT.DigitalOffice.MessageService.Models.Db;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.MessageService.Data
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IDataProvider _provider;

        public WorkspaceRepository(IDataProvider provider)
        {
            _provider = provider;
        }

        public void Add(DbWorkspace workspace)
        {
            _provider.Workspaces.Add(workspace);
            _provider.Save();
        }

        public List<DbWorkspace> Find(FindWorkspaceFilter filter, out int totalCount)
        {
            if (filter.SkipCount < 0)
            {
                throw new BadRequestException("Skip count cannot be less than 0.");
            }

            if (filter.TakeCount < 1)
            {
                throw new BadRequestException("Take count cannot be less than 1.");
            }

            IQueryable<DbWorkspace> workspaces = _provider.Workspaces.AsQueryable();

            if (filter.IsIncludeChannels)
            {
                workspaces = workspaces.Include(w => w.Channels);
            }

            if (!filter.IsIncludeDeactivated)
            {
                workspaces = workspaces.Where(w => w.IsActive);
            }

            totalCount = workspaces.Count();

            return workspaces.Skip(filter.SkipCount).Take(filter.TakeCount).ToList();
        }

        public DbWorkspace Get(Guid workspaceId)
        {
            var result = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
            if (result == null)
            {
                throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
            }

            return result;
        }

        public bool SwitchActiveStatus(Guid workspaceId, bool status)
        {
            DbWorkspace dbWorkspace = _provider.Workspaces.FirstOrDefault(w => w.Id == workspaceId);
            if (dbWorkspace == null)
            {
                throw new NotFoundException($"Workspace with ID '{workspaceId}' was not found.");
            }

            dbWorkspace.IsActive = status;
            dbWorkspace.ModifiedAtUtc = DateTime.UtcNow;

            _provider.Workspaces.Update(dbWorkspace);
            _provider.Save();

            return true;
        }
    }
}