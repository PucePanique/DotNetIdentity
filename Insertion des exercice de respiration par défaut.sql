-- 1. Insérer l'exercice principal
INSERT INTO Exercices (Name, Description)
VALUES ('Cohérence cardiaque', 'Exercice de respiration basé sur 3 phases : inspiration, apnée, expiration');

-- 2. Récupérer l'ID de l'exercice inséré
DECLARE @ExerciceId INT;
SELECT @ExerciceId = Id FROM Exercices WHERE Name = 'Cohérence cardiaque';

-- 3. Insérer les configurations
INSERT INTO ExerciceConfigurations (ExerciceId, Label, InhaleDuration, HoldDuration, ExhaleDuration)
VALUES 
(@ExerciceId, '748', 7, 4, 8),
(@ExerciceId, '55', 5, 0, 5),
(@ExerciceId, '46', 4, 0, 6);
