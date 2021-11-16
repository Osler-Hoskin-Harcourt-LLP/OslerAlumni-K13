-- Check for duplicate main URLs
SELECT 
	NodeGUID, 
	Culture, 
	COUNT(0) AS DuplicateCount
FROM ECA_CustomTable_PageURL
WHERE IsMainURL = 1
GROUP BY NodeGUID, Culture
HAVING COUNT(0) > 1;

-- Delete duplicate main URLs, keeping the most recently updated ones
WITH 
DuplicateMainUrls 
AS
(
	SELECT 
		NodeGUID, 
		Culture
	FROM ECA_CustomTable_PageURL
	WHERE IsMainURL = 1
	GROUP BY NodeGUID, Culture
	HAVING COUNT(0) > 1),
DuplicateRows 
AS (
	SELECT
		ROW_NUMBER() OVER (PARTITION BY u.Culture, u.NodeGUID ORDER BY u.ItemModifiedWhen DESC) AS RN,
		u.ItemID
	FROM ECA_CustomTable_PageURL u
	INNER JOIN DuplicateMainUrls d
	ON u.NodeGUID = d.NodeGUID
	AND u.Culture = d.Culture
	WHERE u.IsMainURL = 1)
DELETE FROM pu
FROM ECA_CustomTable_PageURL pu
INNER JOIN DuplicateRows dr
ON pu.ItemID = dr.ItemID
WHERE dr.RN > 1;

-- Print out the number of affected (deleted) URL records
SELECT @@ROWCOUNT AS '# of deleted URL records';

-- Check for duplicate main URLs again (should be zero)
SELECT 
	NodeGUID, 
	Culture, 
	COUNT(0) AS DuplicateCount
FROM ECA_CustomTable_PageURL
WHERE IsMainURL = 1
GROUP BY NodeGUID, Culture
HAVING COUNT(0) > 1;