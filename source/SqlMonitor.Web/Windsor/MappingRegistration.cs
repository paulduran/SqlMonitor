using AutoMapper;
using Bootstrap;
using SqlMonitor.Core.Domain;
using SqlMonitor.Messages.Commands;

namespace SqlMonitor.Web.Windsor
{
    public class MappingRegistration : IMapCreator
    {
        public void CreateMap()
        {
            Mapper.CreateMap<Query, CreateQuery>();

            Mapper.CreateMap<Query, UpdateQuery>();
        }
    }
}
