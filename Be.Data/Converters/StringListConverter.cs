using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;

namespace Be.Infrastructure.Converters;

public class StringListConverter : ValueConverter<List<string>, string>
{
    public StringListConverter() : base(
        list => list.IsNullOrEmpty() ? null : string.Join(',', list),
        str => str == null ? null : str.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
    ) { }
}