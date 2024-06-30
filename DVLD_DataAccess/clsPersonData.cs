using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
	public class clsPersonData
	{

		public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
		  ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
		   ref short Gendor, ref string Address, ref string Phone, ref string Email,
		   ref int NationalityCountryID, ref string ImagePath)
		{
			bool isFound = false;

			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				//string query = "SELECT * FROM People WHERE PersonID = @PersonID";

				using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByID", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@PersonID", PersonID);

					try
					{
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								// The record was found
								isFound = true;

								FirstName = (string)reader["FirstName"];
								SecondName = (string)reader["SecondName"];

								//ThirdName: allows null in database so we should handle null

								ThirdName = reader["ThirdName"] as string ?? "";


								LastName = (string)reader["LastName"];
								NationalNo = (string)reader["NationalNo"];
								DateOfBirth = (DateTime)reader["DateOfBirth"];
								Gendor = (byte)reader["Gendor"];
								Address = (string)reader["Address"];
								Phone = (string)reader["Phone"];

								//Email: allows null in database so we should handle null
								Email = reader["Email"] as string ?? "";
								NationalityCountryID = (int)reader["NationalityCountryID"];

								//ImagePath: allows null in database so we should handle null
								ImagePath = reader["ImagePath"] as string ?? "";
							}
							else
							{
								// The record was not found
								isFound = false;
							}
						}
					}
					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
						isFound = false;
					}
				}
			}

			return isFound;
		}


		public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
		ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
		 ref short Gendor, ref string Address, ref string Phone, ref string Email,
		 ref int NationalityCountryID, ref string ImagePath)
		{
			bool isFound = false;
			try
			{
				using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
				{
					//string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";
					using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByNationalNo", connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.AddWithValue("@NationalNo", NationalNo);
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								// The record was found
								isFound = true;

								PersonID = (int)reader["PersonID"];
								FirstName = (string)reader["FirstName"];
								SecondName = (string)reader["SecondName"];
								ThirdName = reader["ThirdName"] as string ?? "";
								LastName = (string)reader["LastName"];
								DateOfBirth = (DateTime)reader["DateOfBirth"];
								Gendor = (byte)reader["Gendor"];
								Address = (string)reader["Address"];
								Phone = (string)reader["Phone"];

								//Email: allows null in database so we should handle null
								Email = reader["Email"] as string ?? "";

								NationalityCountryID = (int)reader["NationalityCountryID"];

								//ImagePath: allows null in database so we should handle null
								ImagePath = reader["ImagePath"] as string ?? "";


							}
							else
							{
								// The record was not found
								isFound = false;
							}
						}
					}
				}
			}
			catch (Exception ex) { clsDataAccessLogs.CreateExceptionLog(ex.ToString()); isFound = false; }


			return isFound;
		}



		public static int AddNewPerson(string FirstName, string SecondName,
		   string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
		   short Gendor, string Address, string Phone, string Email,
			int NationalityCountryID, string ImagePath)
		{
			//this function will return the new person id if succeeded and -1 if not.
			int newPersonID = -1;

			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				using (SqlCommand command = new SqlCommand("SP_AddNewPerson", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@FirstName", FirstName);
					command.Parameters.AddWithValue("@SecondName", SecondName);

					if (ThirdName != "" && ThirdName != null)
						command.Parameters.AddWithValue("@ThirdName", ThirdName);
					else
						command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

					command.Parameters.AddWithValue("@LastName", LastName);
					command.Parameters.AddWithValue("@NationalNo", NationalNo);
					command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
					command.Parameters.AddWithValue("@Gendor", Gendor);
					command.Parameters.AddWithValue("@Address", Address);
					command.Parameters.AddWithValue("@Phone", Phone);

					if (Email != "" && Email != null)
						command.Parameters.AddWithValue("@Email", Email);
					else
						command.Parameters.AddWithValue("@Email", System.DBNull.Value);

					command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

					if (ImagePath != "" && ImagePath != null)
						command.Parameters.AddWithValue("@ImagePath", ImagePath);
					else
						command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
					SqlParameter outputIdParam = new SqlParameter("@NewPersonID", SqlDbType.Int) // SP Return NewPersonID ( @NewPersonID INT OUTPUT)
					{
						Direction = ParameterDirection.Output
					};
					command.Parameters.Add(outputIdParam);
					try
					{

						// Execute
						connection.Open();
						command.ExecuteNonQuery();

						// Retrieve the ID of the new person
						newPersonID = (int)command.Parameters["@NewPersonID"].Value;
					}

					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
						return newPersonID;
					}
				}
			}

			return newPersonID;
		}



		public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
		{
			int rowsAffected = 0;
			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				using (SqlCommand command = new SqlCommand("SP_UpdatePerson", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@PersonID", PersonID);
					command.Parameters.AddWithValue("@FirstName", FirstName);
					command.Parameters.AddWithValue("@SecondName", SecondName);
					command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
					command.Parameters.AddWithValue("@LastName", LastName);
					command.Parameters.AddWithValue("@NationalNo", NationalNo);
					command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
					command.Parameters.AddWithValue("@Gendor", Gendor);
					command.Parameters.AddWithValue("@Address", Address);
					command.Parameters.AddWithValue("@Phone", Phone);
					command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
					command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
					command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

					try
					{
						connection.Open();
						rowsAffected = command.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());

					}
				}
			}
			return rowsAffected > 0;
		}



		public static DataTable GetAllPeople()
		{

			DataTable dt = new DataTable();
			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{

				//string query =
				//  @"SELECT People.PersonID, People.NationalNo,
				//          People.FirstName, People.SecondName, People.ThirdName, People.LastName,
				// People.DateOfBirth, People.Gendor,  
				//  CASE
				//              WHEN People.Gendor = 0 THEN 'Male'

				//              ELSE 'Female'

				//              END as GendorCaption ,
				// People.Address, People.Phone, People.Email, 
				//          People.NationalityCountryID, Countries.CountryName, People.ImagePath
				//          FROM            People INNER JOIN
				//                     Countries ON People.NationalityCountryID = Countries.CountryID
				//            ORDER BY People.FirstName";

				using (SqlCommand command = new SqlCommand("SP_GetAllPeople", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					try
					{
						connection.Open();

						SqlDataReader reader = command.ExecuteReader();

						if (reader.HasRows)

						{
							dt.Load(reader);
						}
					}

					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
					}
				}
			}

			return dt;

		}

		public static bool DeletePerson(int PersonID)
		{

			int rowsAffected = 0;

			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				using (SqlCommand command = new SqlCommand("SP_DeletePersonByPersonID", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.AddWithValue("@PersonID", PersonID);
					try
					{
						connection.Open();

						rowsAffected = command.ExecuteNonQuery();

					}
					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
					}
				}
			}

			return (rowsAffected > 0);

		}

		public static bool IsPersonExist(int PersonID)
		{
			bool isFound = false;

			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@PersonID", PersonID);

					try
					{
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							isFound = reader.HasRows;
						}
					}
					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
						isFound = false;
					}
				}

			}

			return isFound;
		}

		public static bool IsPersonExist(string NationalNo)
		{
			bool isFound = false;

			using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
			{
				string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

				using (SqlCommand command = new SqlCommand(query, connection))
				{
					command.Parameters.AddWithValue("@NationalNo", NationalNo);

					try
					{
						connection.Open();
						using (SqlDataReader reader = command.ExecuteReader())
						{
							isFound = reader.HasRows;
						}
					}
					catch (Exception ex)
					{
						clsDataAccessLogs.CreateExceptionLog(ex.ToString());
						isFound = false;
					}
				}
			}
			return isFound;
		}


	}
}
