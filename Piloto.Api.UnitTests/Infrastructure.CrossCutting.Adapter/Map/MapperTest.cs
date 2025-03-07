using AutoMapper;
using Piloto.Api.Infrastructure.CrossCutting.Adapter.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Piloto.Api.UnitTests.Infrastructure.CrossCutting.Adapter.Map
{
    public class MapperTest
    {
       

        [Fact]
        public void ConfigurationIsValid()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingConfiguration()));
            IMapper mapper = mapperConfiguration.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
