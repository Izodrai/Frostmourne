INSERT INTO `trading_hours` (`symbol_id`,`day_of_week`,`all_day`,`t_start`,`t_end`)
VALUES
(1,1,TRUE,"00:00:00","23:59:59"),
(1,2,TRUE,"00:00:00","23:59:59"),
(1,3,TRUE,"00:00:00","23:59:59"),
(1,4,TRUE,"00:00:00","23:59:59"),
(1,5,FALSE,"00:00:00","22:00:00"),
(1,7,FALSE,"22:00:00","23:59:59");


INSERT INTO stock_values (`symbol_id`, `bid_at`, `start_bid_value`, `last_bid_value`, `json_calculation`)
VALUES(1, "2016-12-27 00:00:00", ' 5986', '5709', '{"key1": "value1", "key2": "value2"}');

INSERT INTO `stock_values` (`symbol_id`, `bid_at`, `bid_value`)
VALUES
(1, "2016-12-27 00:00:00", ' 5986'),
(1, "2016-12-27 00:05:00", ' 5978'),
(1, "2016-12-27 00:10:00", ' 4890'),
(1, "2016-12-27 00:15:00", ' 4870'),
(1, "2016-12-27 00:20:00", ' 4858'),
(1, "2016-12-27 00:25:00", ' 5000'),
(1, "2016-12-27 00:30:00", ' 5198'),
(1, "2016-12-27 00:35:00", ' 5580'),
(1, "2016-12-27 00:40:00", ' 5892'),
(1, "2016-12-27 00:45:00", ' 5900'),
(1, "2016-12-27 00:50:00", ' 5838'),
(1, "2016-12-27 00:55:00", ' 5772'),

(1, "2016-12-27 01:00:00", ' 5880'),
(1, "2016-12-27 01:05:00", ' 6040'),
(1, "2016-12-27 01:10:00", ' 6560'),
(1, "2016-12-27 01:15:00", ' 5614'),
(1, "2016-12-27 01:20:00", ' 5718'),
(1, "2016-12-27 01:25:00", ' 5880'),
(1, "2016-12-27 01:30:00", ' 6700'),
(1, "2016-12-27 01:35:00", ' 6400'),
(1, "2016-12-27 01:40:00", ' 6000'),
(1, "2016-12-27 01:45:00", ' 6578'),
(1, "2016-12-27 01:50:00", ' 7338'),
(1, "2016-12-27 01:55:00", ' 7400'),

(1, "2016-12-27 02:00:00", ' 7700'),
(1, "2016-12-27 02:05:00", ' 7730'),
(1, "2016-12-27 02:10:00", ' 8390'),
(1, "2016-12-27 02:15:00", ' 8600'),
(1, "2016-12-27 02:20:00", ' 9200'),
(1, "2016-12-27 02:25:00", ' 10990'),
(1, "2016-12-27 02:30:00", ' 12310'),
(1, "2016-12-27 02:35:00", ' 12000'),
(1, "2016-12-27 02:40:00", ' 11220'),
(1, "2016-12-27 02:45:00", ' 9760'),
(1, "2016-12-27 02:50:00", ' 10100'),
(1, "2016-12-27 02:55:00", ' 10500'),

(1, "2016-12-27 03:00:00", ' 11450'),
(1, "2016-12-27 03:05:00", ' 11700'),
(1, "2016-12-27 03:10:00", ' 12000'),
(1, "2016-12-27 03:15:00", ' 12120'),
(1, "2016-12-27 03:20:00", ' 12800'),
(1, "2016-12-27 03:25:00", ' 12410'),
(1, "2016-12-27 03:30:00", ' 13000'),
(1, "2016-12-27 03:35:00", ' 15000'),
(1, "2016-12-27 03:40:00", ' 16500'),
(1, "2016-12-27 03:45:00", ' 15800'),
(1, "2016-12-27 03:50:00", ' 14190'),
(1, "2016-12-27 03:55:00", ' 15370'),

(1, "2016-12-27 04:00:00", ' 14550'),
(1, "2016-12-27 04:05:00", ' 14350'),
(1, "2016-12-27 04:10:00", ' 15350'),
(1, "2016-12-27 04:15:00", ' 15800'),
(1, "2016-12-27 04:20:00", ' 15200'),
(1, "2016-12-27 04:25:00", ' 14000'),
(1, "2016-12-27 04:30:00", ' 14000'),
(1, "2016-12-27 04:35:00", ' 14900'),
(1, "2016-12-27 04:40:00", ' 15000'),
(1, "2016-12-27 04:45:00", ' 15800'),
(1, "2016-12-27 04:50:00", ' 15750'),
(1, "2016-12-27 04:55:00", ' 15000'),

(1, "2016-12-27 05:00:00", ' 14770'),
(1, "2016-12-27 05:05:00", ' 14000'),
(1, "2016-12-27 05:10:00", ' 14000'),
(1, "2016-12-27 05:15:00", ' 11590'),
(1, "2016-12-27 05:20:00", ' 11500'),
(1, "2016-12-27 05:25:00", ' 10250'),
(1, "2016-12-27 05:30:00", ' 10500'),
(1, "2016-12-27 05:35:00", ' 11800'),
(1, "2016-12-27 05:40:00", ' 12000'),
(1, "2016-12-27 05:45:00", ' 10500'),
(1, "2016-12-27 05:50:00", ' 9000'),
(1, "2016-12-27 05:55:00", ' 9050'),

(1, "2016-12-27 06:00:00", ' 10300'),
(1, "2016-12-27 06:05:00", ' 11900'),
(1, "2016-12-27 06:10:00", ' 10800'),
(1, "2016-12-27 06:15:00", ' 10630'),
(1, "2016-12-27 06:20:00", ' 9200'),
(1, "2016-12-27 06:25:00", ' 9750'),
(1, "2016-12-27 06:30:00", ' 7925'),
(1, "2016-12-27 06:35:00", ' 7500'),
(1, "2016-12-27 06:40:00", ' 8200'),
(1, "2016-12-27 06:45:00", ' 9370'),
(1, "2016-12-27 06:50:00", ' 9490'),
(1, "2016-12-27 06:55:00", ' 9790'),

(1, "2016-12-27 07:00:00", ' 8500'),
(1, "2016-12-27 07:05:00", ' 8580'),
(1, "2016-12-27 07:10:00", ' 8150'),
(1, "2016-12-27 07:15:00", ' 7900'),
(1, "2016-12-27 07:20:00", ' 6850'),
(1, "2016-12-27 07:25:00", ' 6400'),
(1, "2016-12-27 07:30:00", ' 6890'),
(1, "2016-12-27 07:35:00", ' 7150'),
(1, "2016-12-27 07:40:00", ' 6850'),
(1, "2016-12-27 07:45:00", ' 6190'),
(1, "2016-12-27 07:50:00", ' 6110'),
(1, "2016-12-27 07:55:00", ' 6860'),

(1, "2016-12-27 08:00:00", ' 6990'),
(1, "2016-12-27 08:05:00", ' 7100'),
(1, "2016-12-27 08:10:00", ' 7000'),
(1, "2016-12-27 08:15:00", ' 6600'),
(1, "2016-12-27 08:20:00", ' 6960'),
(1, "2016-12-27 08:25:00", ' 7050'),
(1, "2016-12-27 08:30:00", ' 6600'),
(1, "2016-12-27 08:35:00", ' 6600'),
(1, "2016-12-27 08:40:00", ' 6385'),
(1, "2016-12-27 08:45:00", ' 6960'),
(1, "2016-12-27 08:50:00", ' 7100'),
(1, "2016-12-27 08:55:00", ' 6935'),

(1, "2016-12-27 09:00:00", ' 6050'),
(1, "2016-12-27 09:05:00", ' 5500'),
(1, "2016-12-27 09:10:00", ' 5000'),
(1, "2016-12-27 09:15:00", ' 4100'),
(1, "2016-12-27 09:20:00", ' 3800'),
(1, "2016-12-27 09:25:00", ' 3910'),
(1, "2016-12-27 09:30:00", ' 4100'),
(1, "2016-12-27 09:35:00", ' 4650'),
(1, "2016-12-27 09:40:00", ' 5095'),
(1, "2016-12-27 09:45:00", ' 5400'),
(1, "2016-12-27 09:50:00", ' 6200'),
(1, "2016-12-27 09:55:00", ' 5860'),

(1, "2016-12-27", "10:00:00", ' 6000'),
(1, "2016-12-27", "10:05:00", ' 6005'),
(1, "2016-12-27", "10:10:00", ' 6300'),
(1, "2016-12-27", "10:15:00", ' 6390'),
(1, "2016-12-27", "10:20:00", ' 5860'),
(1, "2016-12-27", "10:25:00", ' 5775'),
(1, "2016-12-27", "10:30:00", ' 5200'),
(1, "2016-12-27", "10:35:00", ' 4900'),
(1, "2016-12-27", "10:40:00", ' 4931'),
(1, "2016-12-27", "10:45:00", ' 4530'),
(1, "2016-12-27", "10:50:00", ' 4500'),
(1, "2016-12-27", "10:55:00", ' 4920'),

(1, "2016-12-27", "11:00:00", ' 4710'),
(1, "2016-12-27", "11:05:00", ' 4950'),
(1, "2016-12-27", "11:10:00", ' 4600'),
(1, "2016-12-27", "11:15:00", ' 4401'),
(1, "2016-12-27", "11:20:00", ' 4182'),
(1, "2016-12-27", "11:25:00", ' 4210'),
(1, "2016-12-27", "11:30:00", ' 4200'),
(1, "2016-12-27", "11:35:00", ' 4320'),
(1, "2016-12-27", "11:40:00", ' 4440'),
(1, "2016-12-27", "11:45:00", ' 4410'),
(1, "2016-12-27", "11:50:00", ' 4380'),
(1, "2016-12-27", "11:55:00", ' 4592'),

(1, "2016-12-27", "12:00:00", ' 4400'),
(1, "2016-12-27", "12:05:00", ' 4520'),
(1, "2016-12-27", "12:10:00", ' 4410'),
(1, "2016-12-27", "12:15:00", ' 4650'),
(1, "2016-12-27", "12:20:00", ' 4750'),
(1, "2016-12-27", "12:25:00", ' 4615'),
(1, "2016-12-27", "12:30:00", ' 4610'),
(1, "2016-12-27", "12:35:00", ' 4799'),
(1, "2016-12-27", "12:40:00", ' 4819'),
(1, "2016-12-27", "12:45:00", ' 4680'),
(1, "2016-12-27", "12:50:00", ' 4290'),
(1, "2016-12-27", "12:55:00", ' 4415'),

(1, "2016-12-27", "13:00:00", ' 4340'),
(1, "2016-12-27", "13:05:00", ' 4231'),
(1, "2016-12-27", "13:10:00", ' 4217'),
(1, "2016-12-27", "13:15:00", ' 4000'),
(1, "2016-12-27", "13:20:00", ' 3989'),
(1, "2016-12-27", "13:25:00", ' 3980'),
(1, "2016-12-27", "13:30:00", ' 4050'),
(1, "2016-12-27", "13:35:00", ' 4233'),
(1, "2016-12-27", "13:40:00", ' 4500'),
(1, "2016-12-27", "13:45:00", ' 4490'),
(1, "2016-12-27", "13:50:00", ' 4398'),
(1, "2016-12-27", "13:55:00", ' 4267'),

(1, "2016-12-27", "14:00:00", ' 4211'),
(1, "2016-12-27", "14:05:00", ' 4515'),
(1, "2016-12-27", "14:10:00", ' 4749'),
(1, "2016-12-27", "14:15:00", ' 4630'),
(1, "2016-12-27", "14:20:00", ' 5450'),
(1, "2016-12-27", "14:25:00", ' 5825'),
(1, "2016-12-27", "14:30:00", ' 5850'),
(1, "2016-12-27", "14:35:00", ' 5750'),
(1, "2016-12-27", "14:40:00", ' 6000'),
(1, "2016-12-27", "14:45:00", ' 6200'),
(1, "2016-12-27", "14:50:00", ' 6670'),
(1, "2016-12-27", "14:55:00", ' 6495'),

(1, "2016-12-27", "15:00:00", ' 6750'),
(1, "2016-12-27", "15:05:00", ' 6210'),
(1, "2016-12-27", "15:10:00", ' 6395'),
(1, "2016-12-27", "15:15:00", ' 6390'),
(1, "2016-12-27", "15:20:00", ' 5905'),
(1, "2016-12-27", "15:25:00", ' 5990'),
(1, "2016-12-27", "15:30:00", ' 5600'),
(1, "2016-12-27", "15:35:00", ' 4788'),
(1, "2016-12-27", "15:40:00", ' 3920'),
(1, "2016-12-27", "15:45:00", ' 4090'),
(1, "2016-12-27", "15:50:00", ' 3930'),
(1, "2016-12-27", "15:55:00", ' 3930');