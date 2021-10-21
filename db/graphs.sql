
-- basic stats

-- SELECT DISTINCT(uid) FROM player_actions_log WHERE cid = 4000;

-- player_actions_log
-- dqid from player_quests_log
-- cid, uid, sid, qid, aid, a_detail, log_ts

-- player_quests_log:
-- cid, uid, sessionid, qid, dqid, q_s_id, q_detail, log_q_ts, client_ts

-- player_pageload_log:
-- cid, uid, sessionid, log_ts

-- player_actions_no_quest_log:
-- cid, uid, sessionid, aid, a_detail, log_ts

-- all level play data in one row
-- SELECT *
-- FROM (
--     SELECT 
--         qid,
--         CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
--         CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
--         CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
--         CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
--         CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
--         SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
--         SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
--     FROM (
--         SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
--             ORDER BY aid ASC separator "^") AS lv_details
--         FROM player_actions_log a1
--         WHERE cid = 4000
--         GROUP BY dqid
--     ) AS tmp
-- ) AS level_stats;

-- FILTER OLD LOGS: add this
-- WHERE concat('', reason * 1) != reason




-- uids for A/B test
-- SELECT DISTINCT(uid) AS uid
-- FROM (
--     SELECT uid, a_detail FROM player_actions_no_quest_log l2
--     WHERE l2.cid = 4000 AND l2.aid = 25
--     GROUP BY l2.uid
--     HAVING LENGTH(group_concat(l2.a_detail)) = 1
-- ) AS tmp2
-- WHERE a_detail = "B";

-- tmp queries


-- end tmp

-- NOTE: name should not have special chars "?"


-- A/B Test Overview
SELECT "A-B Test Overview" AS "";
SELECT (CASE WHEN a_detail = "A" THEN 0 ELSE 1 END) AS A_B_Test, count(*) AS num_players
FROM (
    SELECT uid, a_detail FROM player_actions_no_quest_log l2
    WHERE l2.cid = 4000 AND l2.aid = 25
    GROUP BY l2.uid
    HAVING LENGTH(group_concat(l2.a_detail)) = 1
) AS tmp2
GROUP BY a_detail;


SELECT "Total # of Unique Players" AS "";
SELECT 4000 AS cid, COUNT(DISTINCT(uid)) AS count
FROM player_pageload_log
WHERE cid = 4000; 


-- Drop off rate over time
SELECT "Drop-off by Time (Heart Beat) (A)" AS "";
SET @accu := 0;
SELECT heartbeat_min, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT heartbeat_min, count(*) AS num_session
    FROM (
        SELECT sessionid, CAST(MAX(CAST(a_detail AS DECIMAL(10, 2)) / 60) AS SIGNED) AS heartbeat_min
        FROM player_actions_no_quest_log
        WHERE cid = 4000 and aid = 12321 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS tmp2
            WHERE a_detail = "A"
        )
        GROUP BY sessionid
        HAVING heartbeat_min < 30
        ORDER BY heartbeat_min ASC
    ) AS tmp
    GROUP BY heartbeat_min
    ORDER BY heartbeat_min DESC
) AS tmp2;


SELECT "Drop-off by Time (Time Stamp) (A)" AS "";
SET @accu := 0;
SELECT timestamp_min, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT timestamp_min, count(*) AS num_session
    FROM (
        SELECT sessionid, CAST((max(log_ts) - min(log_ts)) / 60.0 AS SIGNED) AS timestamp_min
        FROM player_actions_no_quest_log
        WHERE cid = 4000 and aid = 12321 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY sessionid
        HAVING timestamp_min < 30
        ORDER BY timestamp_min ASC
    ) AS tmp
    GROUP BY timestamp_min
    ORDER BY timestamp_min DESC
) AS tmp2;

-- total time on each level
SELECT "Drop-off by Time (Level Times Sum) (A)" AS "";
SET @ct := 0;
SELECT bin AS minutes, (@ct := @ct + ct) as num_session
FROM (
    SELECT CAST(total_time / 60 AS UNSIGNED) AS bin, count(*) AS ct
    FROM (
        SELECT sid, SUM(CAST(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2) AS UNSIGNED)) total_time, COUNT(*) level_played
        FROM player_actions_log l1
        WHERE cid = 4000 AND aid = 0
            AND CAST(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2) AS UNSIGNED) < 20 * 60 AND uid IN (
                SELECT DISTINCT(uid) AS uid
                FROM (
                    SELECT uid, a_detail FROM player_actions_no_quest_log l2
                    WHERE l2.cid = 4000 AND l2.aid = 25
                    GROUP BY l2.uid
                    HAVING LENGTH(group_concat(l2.a_detail)) = 1
                ) AS abtest_tmp
                WHERE a_detail = "A"
            )
        GROUP BY sid ORDER BY total_time ASC
    ) AS tmp
    GROUP BY bin ORDER BY bin DESC
) AS tmp2;


-- Drop off rate over level / max qid a session reaches
SELECT "Drop-off By Level (Old Iteration #2) (A)" AS "";
SET @accu := 0;
SELECT qid, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT qid, count(*) AS num_session
    FROM (
        SELECT MAX(qid) AS qid
        FROM player_actions_log
        WHERE cid = 4000 AND aid = 1 AND a_detail = '"1"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY sid
    ) AS tmp
    GROUP BY qid ORDER BY qid DESC
) AS tmp2;

SELECT "Drop-off By Level (New Itration #2) (A)" AS "";
SET @accu := 0;
SELECT qid, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT qid, count(*) AS num_session
    FROM (
        SELECT MAX(qid) AS qid
        FROM player_actions_log
        WHERE cid = 4000 AND aid = 5 AND a_detail = '"WON"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY sid
    ) AS tmp
    GROUP BY qid ORDER BY qid DESC
) AS tmp2;


-- 
SELECT "Count of Distinct Players (New) (A)" AS "";
SELECT qid, count(DISTINCT(uid)) distinct_player_ct
FROM player_actions_log
WHERE cid = 4000 AND aid = 5 AND a_detail = '"WON"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
GROUP BY qid ORDER by qid ASC;


-- return rate / number of session per player
select "Return Rate (A)" AS '';
SELECT num_session, COUNT(uid) num_players
FROM (
    SELECT l2.uid, COUNT(DISTINCT(l2.sessionid)) num_session
    FROM player_quests_log l2
    WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
    GROUP BY l2.uid
) AS t2
GROUP BY num_session
ORDER BY num_session DESC;

-- average time to pass each level, include fails
SELECT "Average Level Complete Time (A)" AS "";
SELECT qid, AVG(level_complete_time) AS average_level_complete_time, 
    STDDEV(level_complete_time) AS std_deviation
FROM (
    SELECT qid, SUM(level_try_time) AS level_complete_time
    FROM (
        SELECT sid, qid,
            SUM(CAST((CASE 
                WHEN aid = 0 THEN SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
                ELSE "0" END) 
                AS DECIMAL(10, 2))) AS level_try_time
        FROM player_actions_log
        WHERE cid = 4000 AND (aid = 0 OR aid = 5) AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
        HAVING SUM(case when a_detail = '"WON"' then 1 ELSE 0 end) = 1 
    ) AS tmp
    GROUP BY sid, qid
) AS tmp2
GROUP BY qid ORDER BY qid ASC;


select "(Random) - How long it take to fail (A)" AS '';
-- average time of failed each level
SELECT qid, AVG(time_spent) AS average_time, STDDEV(time_spent) AS std_time
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE (reason = "SPIKE_DEATH" OR reason = "KEY_R")
GROUP BY qid ORDER BY qid ASC;

select "(Random) - How long it take to pass in one try (A)" AS '';
SELECT qid, AVG(time_spent) AS average_time, STDDEV(time_spent) AS std_time
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "WON"
GROUP BY qid ORDER BY qid ASC;


-- average number of pass / failed attempts
select "# of pass and fails (A)" AS '';
SELECT tmp.qid, loss_ct, win_ct 
FROM (
    SELECT qid, COUNT(*) as win_ct
    FROM player_actions_log l1
    WHERE cid = 4000 AND aid = 1 AND a_detail = '"1"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
    GROUP BY qid
) AS tmp, (
    SELECT qid, COUNT(*) as loss_ct
    FROM player_actions_log l1
    WHERE cid = 4000 AND aid = 1 AND a_detail = '"0"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
    GROUP BY qid
) AS tmp2 WHERE tmp.qid = tmp2.qid;


-- average win rate per level, need processing
select "Average Win Rate (A)" AS '';
SELECT qid, 
    SUM(CASE WHEN reason = "WON" THEN 1 ELSE 0 END) / COUNT(*) AS win_rate
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid ORDER by log_ts
    ) AS tmp
) AS level_stats
WHERE concat('', reason * 1) != reason
GROUP BY qid ORDER BY qid;


-- stats 
SELECT "Average Stats By Level (A)" AS "";
SELECT qid, AVG(time_spent) AS average_time, AVG(won) AS average_won, 
    AVG(num_merges) AS average_merges, AVG(num_jump) AS average_jumps,
    AVG(num_walljump) AS average_walljumps
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
GROUP BY qid ORDER BY qid;





-- amount of restarts per level

-- UI actions?

-- Quit rate: can be calculated from Drop-off rate by level
-- max session time: can be calculated from Drop-off rate by time

-- total levels played?
-- select count(DISTINCT(dqid)) from player_actions_log where cid = 4000;




-- spiked locations
SELECT "Spiked Locations (A)" AS "";
SELECT qid, reason_loc AS spiked_position
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "SPIKE_DEATH";


-- key r restart counts
select "Average Manual Restart Count (A)" AS '';
SELECT qid, count(*) as r_restart_ct
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "A"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "KEY_R"
GROUP BY qid;











-- basic stats BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB
-- basic stats BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB
-- basic stats BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB
-- basic stats BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB
-- basic stats BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB


-- Drop off rate over time
SELECT "Drop-off by Time (Heart Beat) (B)" AS "";
SET @accu := 0;
SELECT heartbeat_min, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT heartbeat_min, count(*) AS num_session
    FROM (
        SELECT sessionid, CAST(MAX(CAST(a_detail AS DECIMAL(10, 2)) / 60) AS SIGNED) AS heartbeat_min
        FROM player_actions_no_quest_log
        WHERE cid = 4000 and aid = 12321 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS tmp2
            WHERE a_detail = "B"
        )
        GROUP BY sessionid
        HAVING heartbeat_min < 30
        ORDER BY heartbeat_min ASC
    ) AS tmp
    GROUP BY heartbeat_min
    ORDER BY heartbeat_min DESC
) AS tmp2;


SELECT "Drop-off by Time (Time Stamp) (B)" AS "";
SET @accu := 0;
SELECT timestamp_min, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT timestamp_min, count(*) AS num_session
    FROM (
        SELECT sessionid, CAST((max(log_ts) - min(log_ts)) / 60.0 AS SIGNED) AS timestamp_min
        FROM player_actions_no_quest_log
        WHERE cid = 4000 and aid = 12321 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY sessionid
        HAVING timestamp_min < 30
        ORDER BY timestamp_min ASC
    ) AS tmp
    GROUP BY timestamp_min
    ORDER BY timestamp_min DESC
) AS tmp2;

-- total time on each level
SELECT "Drop-off by Time (Level Times Sum) (B)" AS "";
SET @ct := 0;
SELECT bin AS minutes, (@ct := @ct + ct) as num_session
FROM (
    SELECT CAST(total_time / 60 AS UNSIGNED) AS bin, count(*) AS ct
    FROM (
        SELECT sid, SUM(CAST(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2) AS UNSIGNED)) total_time, COUNT(*) level_played
        FROM player_actions_log l1
        WHERE cid = 4000 AND aid = 0
            AND CAST(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2) AS UNSIGNED) < 20 * 60 AND uid IN (
                SELECT DISTINCT(uid) AS uid
                FROM (
                    SELECT uid, a_detail FROM player_actions_no_quest_log l2
                    WHERE l2.cid = 4000 AND l2.aid = 25
                    GROUP BY l2.uid
                    HAVING LENGTH(group_concat(l2.a_detail)) = 1
                ) AS abtest_tmp
                WHERE a_detail = "B"
            )
        GROUP BY sid ORDER BY total_time ASC
    ) AS tmp
    GROUP BY bin ORDER BY bin DESC
) AS tmp2;


-- Drop off rate over level / max qid a session reaches
SELECT "Drop-off By Level (Old Iteration #2) (B)" AS "";
SET @accu := 0;
SELECT qid, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT qid, count(*) AS num_session
    FROM (
        SELECT MAX(qid) AS qid
        FROM player_actions_log
        WHERE cid = 4000 AND aid = 1 AND a_detail = '"1"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY sid
    ) AS tmp
    GROUP BY qid ORDER BY qid DESC
) AS tmp2;

SELECT "Drop-off By Level (New Itration #2) (B)" AS "";
SET @accu := 0;
SELECT qid, (@accu := @accu + num_session) AS total_sessions
FROM (
    SELECT qid, count(*) AS num_session
    FROM (
        SELECT MAX(qid) AS qid
        FROM player_actions_log
        WHERE cid = 4000 AND aid = 5 AND a_detail = '"WON"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY sid
    ) AS tmp
    GROUP BY qid ORDER BY qid DESC
) AS tmp2;


-- 
SELECT "Count of Distinct Players (New) (B)" AS "";
SELECT qid, count(DISTINCT(uid)) distinct_player_ct
FROM player_actions_log
WHERE cid = 4000 AND aid = 5 AND a_detail = '"WON"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
GROUP BY qid ORDER by qid ASC;


-- return rate / number of session per player
select "Return Rate (B)" AS '';
SELECT num_session, COUNT(uid) num_players
FROM (
    SELECT l2.uid, COUNT(DISTINCT(l2.sessionid)) num_session
    FROM player_quests_log l2
    WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
    GROUP BY l2.uid
) AS t2
GROUP BY num_session
ORDER BY num_session DESC;

-- average time to pass each level, include fails
SELECT "Average Level Complete Time (B)" AS "";
SELECT qid, AVG(level_complete_time) AS average_level_complete_time, 
    STDDEV(level_complete_time) AS std_deviation
FROM (
    SELECT qid, SUM(level_try_time) AS level_complete_time
    FROM (
        SELECT sid, qid,
            SUM(CAST((CASE 
                WHEN aid = 0 THEN SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
                ELSE "0" END) 
                AS DECIMAL(10, 2))) AS level_try_time
        FROM player_actions_log
        WHERE cid = 4000 AND (aid = 0 OR aid = 5) AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
        HAVING SUM(case when a_detail = '"WON"' then 1 ELSE 0 end) = 1 
    ) AS tmp
    GROUP BY sid, qid
) AS tmp2
GROUP BY qid ORDER BY qid ASC;


select "(Random) - How long it take to fail (B)" AS '';
-- average time of failed each level
SELECT qid, AVG(time_spent) AS average_time, STDDEV(time_spent) AS std_time
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE (reason = "SPIKE_DEATH" OR reason = "KEY_R")
GROUP BY qid ORDER BY qid ASC;

select "(Random) - How long it take to pass in one try (B)" AS '';
SELECT qid, AVG(time_spent) AS average_time, STDDEV(time_spent) AS std_time
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "WON"
GROUP BY qid ORDER BY qid ASC;


-- average number of pass / failed attempts
select "# of pass and fails (B)" AS '';
SELECT tmp.qid, loss_ct, win_ct 
FROM (
    SELECT qid, COUNT(*) as win_ct
    FROM player_actions_log l1
    WHERE cid = 4000 AND aid = 1 AND a_detail = '"1"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
    GROUP BY qid
) AS tmp, (
    SELECT qid, COUNT(*) as loss_ct
    FROM player_actions_log l1
    WHERE cid = 4000 AND aid = 1 AND a_detail = '"0"' AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
    GROUP BY qid
) AS tmp2 WHERE tmp.qid = tmp2.qid;


-- average win rate per level, need processing
select "Average Win Rate (B)" AS '';
SELECT qid, 
    SUM(CASE WHEN reason = "WON" THEN 1 ELSE 0 END) / COUNT(*) AS win_rate
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid ORDER by log_ts
    ) AS tmp
) AS level_stats
WHERE concat('', reason * 1) != reason
GROUP BY qid ORDER BY qid;


-- stats 
SELECT "Average Stats By Level (B)" AS "";
SELECT qid, AVG(time_spent) AS average_time, AVG(won) AS average_won, 
    AVG(num_merges) AS average_merges, AVG(num_jump) AS average_jumps,
    AVG(num_walljump) AS average_walljumps
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
GROUP BY qid ORDER BY qid;





-- amount of restarts per level

-- UI actions?

-- Quit rate: can be calculated from Drop-off rate by level
-- max session time: can be calculated from Drop-off rate by time

-- total levels played?
-- select count(DISTINCT(dqid)) from player_actions_log where cid = 4000;




-- spiked locations
SELECT "Spiked Locations (B)" AS "";
SELECT qid, reason_loc AS spiked_position
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "SPIKE_DEATH";


-- key r restart counts
select "Average Manual Restart Count (B)" AS '';
SELECT qid, count(*) as r_restart_ct
FROM (
    SELECT 
        qid,
        CAST(SUBSTRING_INDEX(lv_details, "^", 1) AS DECIMAL(10, 2)) AS time_spent,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 2), "^", -1) AS UNSIGNED) AS won,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 3), "^", -1) AS UNSIGNED) AS num_merges,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 4), "^", -1) AS UNSIGNED) AS num_jump,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 5), "^", -1) AS UNSIGNED) AS num_walljump,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 6), "^", -1) AS reason,
        SUBSTRING_INDEX(SUBSTRING_INDEX(lv_details, "^", 7), "^", -1) AS reason_loc
    FROM (
        SELECT qid, group_concat(SUBSTRING(a_detail, 2, LENGTH(a_detail) - 2)
            ORDER BY aid ASC separator "^") AS lv_details
        FROM player_actions_log a1
        WHERE cid = 4000 AND uid IN (
            SELECT DISTINCT(uid) AS uid
            FROM (
                SELECT uid, a_detail FROM player_actions_no_quest_log l2
                WHERE l2.cid = 4000 AND l2.aid = 25
                GROUP BY l2.uid
                HAVING LENGTH(group_concat(l2.a_detail)) = 1
            ) AS abtest_tmp
            WHERE a_detail = "B"
        )
        GROUP BY dqid
    ) AS tmp
) AS level_stats
WHERE reason = "KEY_R"
GROUP BY qid;







