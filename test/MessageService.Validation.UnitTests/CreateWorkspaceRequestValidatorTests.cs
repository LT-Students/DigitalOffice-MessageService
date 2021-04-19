using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Workspace;
using NUnit.Framework;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
    public class CreateWorkspaceRequestValidatorTests
    {
        private IValidator<WorkspaceRequest> _validator;
        private WorkspaceRequest _workspace;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateWorkspaceValidator();

            _workspace = new WorkspaceRequest
            {
                Name = "Name",
                Description = "Description"
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenNameIsNull()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            _validator.TestValidate(_workspace).ShouldNotHaveAnyValidationErrors();
        }
    }
}
