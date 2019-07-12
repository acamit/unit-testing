namespace MediaStorage.Common
{
    public class Constants
    {
        public const string NoRecordsExists = "No records exist";
        public const string SelectedSubCategoriesDoesNotExistErrorMessage = "Selected sub categories does not exist.";
        public const string ErrorWhileInsertingRecordErrorMessage = "Error while inserting record";
        public const string MissingIdErrorMessage = "Id is requid for updating a record";
        public const string InvalidIdErrorMessage = "Invalid id.";

        #region DepartmentRelatedMessages
        public const string DepartmentAddFailedMessage = "Error while inserting department";
        public const string DepartmentAddSuccessMessage = "Department added successfully";
        public const string UpdateDepartmentSuccessMessage = "Department updated successfully.";
        public const string UpdateDepartmentFailedMessage = "Error while updating department.";
        public const string DeleteDepartmentFailedMessage = "Error while deleting department.";
        public const string DeleteDepartmentSuccessMessage = "Department deleted successfully.";
        #endregion

        #region LibraryServiceRelatedMessages
        public const string LibraryAddFailedMessage = "Error while inserting library.";
        public const string LibraryAddSuccessMessage = "Library added successfully";
        public const string UpdateLibrarySuccessMessage = "Library updated successfully.";
        public const string UpdateLibraryFailedMessage = "Error while updating Library.";
        public const string DeleteLibraryFailedMessage = "Error while deleting Library.";
        public const string DeleteLibrarySuccessMessage = "Library deleted successfully.";
        #endregion

        #region MaterialServiceRealtedMessages
        public const string MaterialAddFailedMessage = "Error while inserting material.";
        public const string MaterialAddSuccessMessage = "Material added successfully";
        public const string UpdateMaterialSuccessMessage = "Material updated successfully.";
        public const string UpdateMaterialFailedMessage = "Error while updating material.";
        public const string DeleteMaterialFailedMessage = "Error while deleting material.";
        public const string DeleteMaterialSuccessMessage = "Material deleted successfully.";
        #endregion

        #region UserServiceRelatedMessages
        public const string UserNotFoundMessage = "User not found.";
        public const string LoginSuccessMessage = "Login successful.";
        public const string UserNameAlreadyExistsMessage = "Username already exist.";
        public const string MailAlReadyExistsMessage = "Mail already exist.";
        public const string DeleteUserFailedMessage = "Error while deleting User.";
        public const string DeleteUserSuccessMessage = "User deleted successfully.";
        public const string AddUserFailedMessage  = "Add user failed.";
        public const string AddUserSuccessMessage = "Add user successful.";
        #endregion
    }
}
