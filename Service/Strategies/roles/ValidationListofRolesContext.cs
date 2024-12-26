
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.userRoles;

    public interface IValidationListofRolesContext
    {
        bool Validate(List<int> roleIds);
    }

    public class ValidationListofRolesContext : IValidationListofRolesContext
    {
        private readonly IValidateRolesStrategy? _validateRolesStrategy;

        public ValidationListofRolesContext(IValidateRolesStrategy? validateRolesStrategy)
        {
            _validateRolesStrategy = validateRolesStrategy ?? new DefaultValidationRolesStrategy();
        }

        public bool Validate(List<int> roleIds)
        {
            if (_validateRolesStrategy == null)
            {
                throw new ArgumentNullException(nameof(_validateRolesStrategy));
            }
            return _validateRolesStrategy.Validate(roleIds);
        }
    }

