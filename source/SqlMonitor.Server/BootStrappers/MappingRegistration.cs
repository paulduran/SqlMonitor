using AutoMapper;
using Bootstrap;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Commands;
using SqlMonitor.Messages.Events;

namespace SqlMonitor.Server.BootStrappers
{
    public class MappingRegistration : IMapCreator
    {
        public void CreateMap()
        {
            Mapper.CreateMap<CreateQuery, Query>();

            Mapper.CreateMap<UpdateQuery, Query>();

            Mapper.CreateMap<IQueryResult, QueryResult>();

            Mapper.CreateMap<Query, ScheduleQuery>();
        }
    }
}
