USE market;

CREATE TABLE symbols (
	`id` 			TINYINT 	UNSIGNED NOT NULL AUTO_INCREMENT,
	`reference` 	VARCHAR(10) COLLATE utf8_bin NOT NULL,
	`description`	TEXT,
	`active`		BOOLEAN NOT NULL DEFAULT FALSE,
	PRIMARY KEY (`id`),
	UNIQUE KEY `reference` (`reference`)
);

INSERT INTO `symbols` (`id`, `reference`, `description`, `active`)
VALUES 
(1, "EURUSD", "Instrument, which price is based on quotations of Euro to American Dollar on the interbank market.", true),
(2, "EURGBP", "Instrument, which price is based on quotations of Euro to British Pound on the interbank market.", false),
(3, "GBPUSD", "Instrument, which price is based on quotations of British Pound to American Dollar on the interbank market.", false);

CREATE OR REPLACE VIEW `v_activ_symbols`
AS SELECT `s`.`id`, `s`.`reference`, `s`.`description`
FROM `symbols` AS s
WHERE `s`.`active` 
ORDER BY `s`.`id` ASC;

CREATE TABLE `stock_values` (
 	`id` 				INT 		UNSIGNED NOT NULL AUTO_INCREMENT,
 	`symbol_id` 		TINYINT 	UNSIGNED  NOT NULL,
 	`bid_at` 			DATETIME 	NOT NULL,
 	`start_bid_value` 	MEDIUMINT 	UNSIGNED NOT NULL,
 	`last_bid_value` 	MEDIUMINT 	UNSIGNED NOT NULL,
 	`created_at` 		DATETIME 	NOT NULL,
 	`updated_at` 		DATETIME 	NOT NULL,
 	PRIMARY KEY (`id`),
	CONSTRAINT stock_values_symbol_types_reference FOREIGN KEY symbol_id(symbol_id) REFERENCES symbols (id),
 	UNIQUE KEY `symbol` (`symbol_id`,`bid_at`),
 	KEY `bid_at` (`bid_at`)
);

CREATE TABLE `stock_analyse` (
	`id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
	`stock_value_id` INT UNSIGNED NOT NULL,
	`sma_c` DECIMAL(14,4) NOT NULL,
	`sma_l` DECIMAL(14,4) NOT NULL,
	`ema_c` DECIMAL(14,4) NOT NULL,
	`ema_l` DECIMAL(14,4) NOT NULL,
	`macd_value` DECIMAL(14,4) NOT NULL,
	`macd_trigger`  DECIMAL(14,4) NOT NULL,
	`macd_signal`  DECIMAL(14,4) NOT NULL,
 	PRIMARY KEY (`id`),
	CONSTRAINT stock_values_id_reference FOREIGN KEY stock_value_id(stock_value_id) REFERENCES stock_values (id)
);

CREATE OR REPLACE VIEW `v_last_2_days_stock_values`
AS SELECT 
`sv`.`id` AS `sv_id`, `sv`.`bid_at`, `sv`.`start_bid_value`, `sv`.`last_bid_value`, 
`s`.`id` AS `s_id`, `s`.`reference`, 
`sa`.`id` AS `sa_id`, `sa`.`sma_c`, `sa`.`sma_l`, `sa`.`ema_c`, `sa`.`ema_l`, `sa`.`macd_value`, `sa`.`macd_trigger`, `sa`.`macd_signal`
FROM `stock_values` AS `sv`
JOIN `symbols` AS `s` ON `s`.`id` = `sv`.`symbol_id`
LEFT  JOIN `stock_analyse` AS `sa` ON `sv`.`id` = `sa`.`stock_value_id`
WHERE `sv`.`bid_at` >= DATE_ADD(NOW(), INTERVAL -2 DAY);

CREATE OR REPLACE VIEW `v_last_10_days_stock_values`
AS SELECT 
`sv`.`id` AS `sv_id`, `sv`.`bid_at`, `sv`.`start_bid_value`, `sv`.`last_bid_value`, 
`s`.`id` AS `s_id`, `s`.`reference`, 
`sa`.`mm_c`, `sa`.`mm_l`
FROM `stock_values` AS `sv`
JOIN `symbols` AS `s` ON `s`.`id` = `sv`.`symbol_id`
LEFT  JOIN `stock_analyse` AS `sa` ON `sv`.`id` = `sa`.`stock_value_id`
WHERE `sv`.`bid_at` >= DATE_ADD(NOW(), INTERVAL -10 DAY);

CREATE TABLE `days` (
	`id`	TINYINT UNSIGNED NOT NULL,
	`name`	VARCHAR(10) NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `name`(`name`)
);

INSERT INTO `days` (`id`,`name`)
VALUES
(1, "Lundi"),
(2, "Mardi"),
(3, "Mercredi"),
(4, "Jeudi"),
(5, "Vendredi"),
(6, "Samedi"),
(7, "Dimanche");

CREATE TABLE `trading_hours` (
	`id`			SMALLINT	UNSIGNED NOT NULL AUTO_INCREMENT,
	`symbol_id`		TINYINT 	UNSIGNED NOT NULL,
	`day_of_week` 	TINYINT		UNSIGNED NOT NULL,
	`all_day`		BOOLEAN		DEFAULT FALSE,
	`t_start`		TIME		DEFAULT "00:00:00",
	`t_end`			TIME		DEFAULT "23:59:59",
	PRIMARY KEY (`id`),
	CONSTRAINT trading_hours_symbol_types_reference FOREIGN KEY symbol_id(symbol_id) REFERENCES symbols (id),
	CONSTRAINT trading_hours_days_reference FOREIGN KEY day_of_week(day_of_week) REFERENCES days (id),
	UNIQUE KEY `day_symbol` (`symbol_id`,`day_of_week`)
);

CREATE OR REPLACE VIEW `v_trading_hours`
AS SELECT `s`.`reference`, `d`.`name`, `th`.`all_day`, `th`.`t_start`, `th`.`t_end`
FROM `trading_hours` AS th
JOIN days AS d ON `d`.`id` = `th`.`day_of_week`
JOIN symbols AS s ON `s`.`id` = `th`.`symbol_id`;

/*
CREATE TABLE type_users (
	`id`	SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
	`name`	VARCHAR(15),
	PRIMARY KEY (`id`),
	UNIQUE KEY `type_user_name` (`name`)
);

INSERT INTO type_users (`id`,`name`)
VALUES 
(1, "data retriever");

CREATE TABLE users (
	`id`		SMALLINT		UNSIGNED NOT NULL AUTO_INCREMENT,
	`type_id`	SMALLINT		UNSIGNED NOT NULL,
	`login`		VARCHAR(15)		NOT NULL,
	`local_pwd`	VARCHAR(255) 	NOT NULL,
	`xtb_login`	SMALLINT		NOT NULL,
	`xtb_pwd`	SMALLINT		NOT NULL,
	PRIMARY KEY (`id`),
	CONSTRAINT `users_type_name` FOREIGN KEY `type_id`(`type_id`) REFERENCES `type_users` (`id`),
	UNIQUE KEY `user_login` (`login`)
);
*/
