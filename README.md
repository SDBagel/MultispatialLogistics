# MultispatialLogistics
Bringing EVE services to you when you need them.

Multispatial Logistics provides never-before-seen services making your gameplay experience in EVE Online that much better.
Included in this repository are the python script to extract stargate data from the SDE, and the main ASP.NET Core 2.1 application.

Planned features:
Route time calculation given ship, rigs, jump capability, character skill, and jump bridges

Todo:
Add ship db for getting align times
Align times in route time calculation
Frontend design concepts

Known code issues:
stargates db for parent system names is inefficient and should be moved to separate table
stargates db is still editable by publically accessible views
