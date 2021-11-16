-- Initial check
SELECT OnePlaceReference, *
FROM CMS_User
WHERE OnePlaceReference IS NOT NULL
AND (LEN(OnePlaceReference) != 18
OR PATINDEX('%[^0-9a-z]%', OnePlaceReference) > 0)

-- Removing incorrect references
UPDATE CMS_User
SET OnePlaceReference = NULL
WHERE OnePlaceReference IS NOT NULL
AND (LEN(OnePlaceReference) != 18
OR PATINDEX('%[^0-9a-z]%', OnePlaceReference) > 0)

-- Post-update check
SELECT OnePlaceReference, *
FROM CMS_User
WHERE OnePlaceReference IS NOT NULL
AND (LEN(OnePlaceReference) != 18
OR PATINDEX('%[^0-9a-z]%', OnePlaceReference) > 0)