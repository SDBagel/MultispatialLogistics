# SDEScanner

The SDEScanner script uses the MSSQL dump provided by CCP Games, hf. and sorts relevant data used by MultispatialLogistics into a JSON format. This is readable by the DatabaseInitializer.cs class and all that is needed is for the file to be named accordingly and placed in the Data folder. The legacy version of this script does the same thing, except to a .txt file that needs to be inserted into the SeedData.cs class directly.

It is recommended that the JSON format be used with the .Core3 version of MultispatialLogistics, while the .txt version be used with the Core 2.1 version of MultispatialLogistics due to differences in how the database gets seeded.

# Important Notes

There is one really annoying issue in the EVE SDE - three coordinate values have more than one decimal place. THis results in the script not fully purifying the data and DatabaseInitializer running into value conversion issues. A future fix to the SDEScanner script could resolve this issue, however, at the moment, you will need to find "." using control F in the generated JSON file and delete trailing decimal places. This prevents the DatabaseInitializer from running into any issues.

Since the file is large, I recommend you use the Windows built-in Notepad, as it will open the file without any issues and has everything needed.
