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
    }
}
