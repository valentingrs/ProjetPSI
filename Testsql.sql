INSERT INTO Tiers (IDTiers, MotDePasse, CodePostal, Ville, Email, Tel, Nom, Adresse, Prenom) VALUES
-- Clients
(1, 'root', '75000', 'Paris', 'admin@email.com', '0123456789', 'Admin', 'Pole Léonard de Vinci', 'Admin'),
(2, 'pass456', '75002', 'Paris', 'marie.lefebvre@gmail.com', '0623456789', 'Lefebvre', '45 Rue Montorgueil', 'Marie'),
(3, 'pass789', '75003', 'Paris', 'pierre.martin@gmail.com', '0634567890', 'Martin', '22 Rue de Birague', 'Pierre'),
(4, 'pass101', '75004', 'Paris', 'sophie.durand@gmail.com', '0645678901', 'Durand', '78 Quai des Orfèvres', 'Sophie'),
(5, 'pass202', '75005', 'Paris', 'luc.bernard@gmail.com', '0656789012', 'Bernard', '15 Rue Monge', 'Luc'),
(6, 'pass303', '75006', 'Paris', 'claire.robert@gmail.com', '0667890123', 'Robert', '33 Rue de Vaugirard', 'Claire'),
(7, 'pass404', '75007', 'Paris', 'thomas.leroy@gmail.com', '0678901234', 'Leroy', '19 Avenue de la Bourdonnais', 'Thomas'),
(8, 'pass505', '75008', 'Paris', 'julie.girard@gmail.com', '0689012345', 'Girard', '66 Avenue des Champs-Élysées', 'Julie'),
(9, 'pass606', '75009', 'Paris', 'antoine.fournier@gmail.com', '0690123456', 'Fournier', '27 Rue du Faubourg Montmartre', 'Antoine'),
(10, 'pass707', '75010', 'Paris', 'emma.richard@gmail.com', '0601234567', 'Richard', '88 Boulevard de Magenta', 'Emma'),
-- Cuisiniers
(11, 'pass808', '75011', 'Paris', 'lucas.moreau@gmail.com', '0613456789', 'Moreau', '44 Rue Oberkampf', 'Lucas'),
(12, 'pass909', '75012', 'Paris', 'amelie.petit@gmail.com', '0624567890', 'Petit', '29 Avenue Daumesnil', 'Amelie'),
(13, 'pass010', '75013', 'Paris', 'hugo.dubois@gmail.com', '0635678901', 'Dubois', '57 Avenue d’Italie', 'Hugo'),
(14, 'pass111', '75014', 'Paris', 'lea.simon@gmail.com', '0646789012', 'Simon', '92 Rue d’Alésia', 'Lea'),
(15, 'pass222', '75015', 'Paris', 'nathan.guerin@gmail.com', '0657890123', 'Guerin', '105 Rue de Vaugirard', 'Nathan'),
(16, 'pass333', '75016', 'Paris', 'eva.laurent@gmail.com', '0668901234', 'Laurent', '8 Avenue Kléber', 'Eva'),
(17, 'pass444', '75017', 'Paris', 'maxime.vincent@gmail.com', '0679012345', 'Vincent', '16 Rue de Courcelles', 'Maxime');

-- Insertion des Clients
INSERT INTO Client (IDClient) VALUES
(1), (2), (3), (4), (5), (6), (7), (8), (9), (10);

-- Insertion des Cuisiniers
INSERT INTO Cuisinier (IDCuisinier) VALUES
(11), (12), (13), (14), (15), (16), (17);

-- Insertion des Plats
INSERT INTO Plat (IDPlat, NomPlat, TypePlat, Ingredients, Nationalite, Regime) VALUES
(1, 'Boeuf Bourguignon', 'Plat principal', 'Boeuf, Vin, Carottes', 'Française', 'Standard'),
(2, 'Ratatouille', 'Plat principal', 'Aubergine, Courgette, Poivrons', 'Française', 'Végétarien'),
(3, 'Crème Brûlée', 'Dessert', 'Crème, Sucre, Vanille', 'Française', 'Standard'),
(4, 'Sushi', 'Plat principal', 'Riz, Poisson, Algue', 'Japonaise', 'Standard'),
(5, 'Pad Thai', 'Plat principal', 'Nouilles, Crevettes, Cacahuètes', 'Thaïlandaise', 'Standard'),
(6, 'Pizza Margherita', 'Plat principal', 'Tomate, Mozzarella, Basilic', 'Italienne', 'Végétarien'),
(7, 'Tiramisu', 'Dessert', 'Mascarpone, Café, Cacao', 'Italienne', 'Standard'),
(8, 'Poulet au beurre', 'Plat principal', 'Poulet, Beurre, Crème', 'Indienne', 'Standard'),
(9, 'Naan', 'Accompagnement', 'Farine, Levure, Beurre', 'Indienne', 'Végétarien'),
(10, 'Falafel', 'Plat principal', 'Pois chiches, Épices, Herbes', 'Moyen-Orientale', 'Végan'),
(11, 'Houmous', 'Accompagnement', 'Pois chiches, Tahini, Citron', 'Moyen-Orientale', 'Végan'),
(12, 'Paella', 'Plat principal', 'Riz, Fruits de mer, Safran', 'Espagnole', 'Standard'),
(13, 'Churros', 'Dessert', 'Pâte, Sucre, Cannelle', 'Espagnole', 'Végétarien'),
(14, 'Soupe Miso', 'Soupe', 'Miso, Tofu, Algue', 'Japonaise', 'Végétarien'),
(15, 'Kimchi', 'Accompagnement', 'Chou, Piment, Ail', 'Coréenne', 'Végan'),
(16, 'Tacos', 'Plat principal', 'Tortilla, Boeuf, Salsa', 'Mexicaine', 'Standard'),
(17, 'Guacamole', 'Accompagnement', 'Avocat, Citron vert, Tomate', 'Mexicaine', 'Végan'),
(18, 'Ceviche', 'Plat principal', 'Poisson, Citron vert, Piment', 'Péruvienne', 'Standard'),
(19, 'Baklava', 'Dessert', 'Feuilles de phyllo, Noix, Miel', 'Turque', 'Végétarien'),
(20, 'Poutine', 'Plat principal', 'Frites, Fromage, Sauce', 'Canadienne', 'Standard');

-- Insertion des PlatCuisinier (30 plats, 10 restent disponibles)
INSERT INTO PlatCuisinier (IDPlatCuisinier, IDPlat, DateFabrication, DatePeremption, IngredientsSupp, PrixPlat, NombrePersonnes, IDCuisinier) VALUES
(1, 1, '2025-03-20', '2025-03-25', 'Champignons', 15.00, 2, 11),
(2, 2, '2025-03-22', '2025-03-27', 'Olives', 10.00, 2, 12),
(3, 3, '2025-03-25', '2025-03-30', 'Caramel', 8.00, 1, 13),
(4, 4, '2025-03-28', '2025-04-02', 'Avocat', 12.00, 2, 14),
(5, 5, '2025-04-01', '2025-04-06', 'Tofu', 11.00, 2, 15),
(6, 6, '2025-04-03', '2025-04-08', 'Origan', 9.00, 2, 16),
(7, 7, '2025-04-05', '2025-04-10', 'Chocolat', 7.00, 1, 17),
(8, 8, '2025-04-07', '2025-04-12', 'Yaourt', 14.00, 2, 11),
(9, 9, '2025-04-10', '2025-04-15', 'Ail', 5.00, 2, 12),
(10, 10, '2025-04-12', '2025-04-17', 'Yaourt', 8.00, 2, 13),
(11, 11, '2025-04-15', '2025-04-20', 'Huile d’olive', 6.00, 2, 14),
(12, 12, '2025-04-18', '2025-04-23', 'Poulet', 16.00, 3, 15),
(13, 13, '2025-04-20', '2025-04-25', 'Chocolat', 6.00, 1, 16),
(14, 14, '2025-04-22', '2025-04-27', 'Oignons verts', 5.00, 2, 17),
(15, 15, '2025-04-25', '2025-04-30', 'Carottes', 4.00, 2, 11),
(16, 16, '2025-04-27', '2025-05-02', 'Fromage', 10.00, 2, 12),
(17, 17, '2025-04-30', '2025-05-05', 'Oignons', 6.00, 2, 13),
(18, 18, '2025-05-02', '2025-05-07', 'Maïs', 12.00, 2, 14),
(19, 19, '2025-05-05', '2025-05-10', 'Pistaches', 8.00, 2, 15),
(20, 20, '2025-05-07', '2025-05-12', 'Bacon', 9.00, 2, 16);

-- Insertion des Commandes (25 commandes)
INSERT INTO Commande (IDCommande, DateCommande, HeureCommande, IDClient, IDCuisinier) VALUES
-- Cuisinier 11 : 5 commandes
(1, '2025-03-21', '12:00:00', 1, 11),
(2, '2025-03-23', '18:30:00', 2, 11),
(3, '2025-04-08', '13:00:00', 3, 11),
(4, '2025-04-26', '19:00:00', 4, 11),
(5, '2025-05-01', '12:30:00', 5, 11),
-- Cuisinier 12 : 4 commandes
(6, '2025-03-24', '17:00:00', 6, 12),
(8, '2025-04-28', '12:00:00', 8, 12),
-- Cuisinier 13 : 3 commandes
(10, '2025-03-26', '12:30:00', 10, 13),
(11, '2025-04-13', '18:30:00', 1, 13),
(12, '2025-05-04', '13:00:00', 2, 13),
-- Cuisinier 14 : 3 commandes
(13, '2025-03-29', '19:00:00', 3, 14),
(14, '2025-04-16', '12:00:00', 4, 14),
(15, '2025-05-06', '18:00:00', 5, 14),
-- Cuisinier 15 : 2 commandes
(16, '2025-04-02', '12:30:00', 6, 15),
-- Cuisinier 16 : 2 commandes
(18, '2025-04-04', '18:00:00', 8, 16),
(19, '2025-04-21', '12:00:00', 9, 16),
-- Cuisinier 17 : 1 commande
(20, '2025-04-06', '13:00:00', 10, 17),
-- Client 1 : 4 commandes
(22, '2025-04-23', '12:30:00', 1, 16),
(23, '2025-05-07', '18:30:00', 1, 14),
-- Commandes supplémentaires
(24, '2025-04-14', '12:00:00', 2, 17)

-- Insertion des PlatCommande (15 plats attribués aux commandes, 15 restent disponibles)
INSERT INTO PlatCommande (IDCommande, IDPlatCuisinier) VALUES
(1, 1),
(4, 15),
(10, 3),
(11, 10),
(12, 17),
(13, 4),
(14, 11),
(15, 18),
(16, 5),
(18, 6),
(19, 13),
(20, 7);