#!/usr/bin/python3
import os

import sys
sys.path.insert(0, "../matplotinstall")

import matplotlib.pyplot as plt


def sum_normalize(arr):
    accu = sum(arr)
    return [arr[i] / accu for i in range(len(arr))]
        

def head_normalize(arr):
    if arr[0] == 0:
        print("zero", arr)
        return arr[:]
    return [arr[i] / arr[0] for i in range(len(arr))]


def plot_and_save_graph(save_dir, title, column_names, column_data):
    print('AB plot:', title)

    if len(column_names) == 2:
        width = 0.35
    elif len(column_names) == 3:
        width = 0.25
    else:
        width = 0.15



    x_label = column_data[0] 
    i_offset = int(len(column_names) / 2)

    if "(B)" in title:
        title = title.replace("(B)", "(AB)")
        # both data
        x_label_B = column_data[i_offset]

        for i in range(1, i_offset):
            plt.bar(x_label, column_data[i], width, label=column_names[i] + " (A)")
            # offset
            for l_i in range(len(x_label)):
                x_label[l_i] += width
            for l_i in range(len(x_label_B)):
                x_label_B[l_i] += width

            plt.bar(x_label_B, column_data[i + i_offset], width, label=column_names[i] + " (B)")
            for l_i in range(len(x_label)):
                x_label[l_i] += width
            for l_i in range(len(x_label_B)):
                x_label_B[l_i] += width
                
    else:
        # normal graph
        for i in range(1, len(column_names)):
            plt.bar(x_label, column_data[i], width, label=column_names[i])
            # offset
            for l_i in range(len(x_label)):
                x_label[l_i] += width

    if column_names[0] == "qid":
        plt.xticks(ticks=[i for i in range(1, 11)])

    plt.title(title)
    plt.xlabel(column_names[0])
    plt.ylabel(column_names[1])
    plt.legend(loc='best')

    if not os.path.exists(save_dir):
        print("creating dir", save_dir)
        os.makedirs(save_dir)
    save_path = save_dir + title + ".png"
    plt.savefig(save_path)
    plt.clf()

    # split level stats
    # if "Average Stats By Level (AB)" in title:
    #     for col_idx in range(1, i_offset):
    #         # A
    #         new_col_names = [column_names[0], column_names[col_idx] + " A "]
    #         new_col_data = [column_data[0], column_data[col_idx]]
    #         new_col_names += [column_names[col_idx + i_offset] + " B "]
    #         new_col_data += [column_data[col_idx + i_offset]]
    #         plot_and_save_graph_normal(save_dir, title + "(" + new_col_names[1] + ")", new_col_names, new_col_data)

def plot_helper(save_dir, title, column_names, column_data):
    print('normal plot:', title)

    width = 1 / (len(column_names) + 1)

    x_label = column_data[0][:]

    i_offset = int(len(column_names) / 2)

    if "(AB)" in title:
        width = 1 / (i_offset + 1)
        # both queries
        x_label_B = column_data[i_offset][:]
        for i in range(1, i_offset):
            plt.bar(x_label, column_data[i], width, label=column_names[i] + " (A)")
            # offset
            for l_i in range(len(x_label)):
                x_label[l_i] += width
            for l_i in range(len(x_label_B)):
                x_label_B[l_i] += width

            plt.bar(x_label_B, column_data[i + i_offset], width, label=column_names[i + i_offset] + " (B)")
            for l_i in range(len(x_label)):
                x_label[l_i] += width
            for l_i in range(len(x_label_B)):
                x_label_B[l_i] += width
    else:
        # normal graph
        for i in range(1, len(column_names)):
            plt.bar(x_label, column_data[i], width, label=column_names[i])
            # offset
            for l_i in range(len(x_label)):
                x_label[l_i] += width

    if column_names[0] == "qid":
        plt.xticks(ticks=[i for i in range(1, 13)])

    plt.title(title)
    plt.xlabel(column_names[0])
    plt.ylabel(column_names[1])
    plt.legend(loc='best')

    if not os.path.exists(save_dir):
        print("creating dir", save_dir)
        os.makedirs(save_dir)
    save_path = save_dir + title + ".png"
    plt.savefig(save_path)
    plt.clf()

def plot_and_save_graph_normal(save_dir, title, column_names, column_data):
    plot_helper(save_dir, title, column_names, column_data)
    # normalize and plot it
    if "(AB)" in title:
        new_col_data = [column_data[0][:]]
        for i in range(1, int(len(column_data) / 2)):
            new_col_data += [sum_normalize(column_data[i])]

        new_col_data += [column_data[int(len(column_data) / 2)][:]]
        for i in range(int(len(column_data) / 2) + 1, len(column_data)):
            new_col_data += [sum_normalize(column_data[i])]
        plot_helper(save_dir, "-normed- " + title, column_names, new_col_data)
    else:
        new_col_data = [column_data[0][:]]
        for i in range(1, len(column_data)):
            new_col_data += [sum_normalize(column_data[i])]
        plot_helper(save_dir, "-normed- " + title, column_names, new_col_data)
    # start norm
    if "(AB)" in title:
        new_col_data = [column_data[0][:]]
        for i in range(1, int(len(column_data) / 2)):
            new_col_data += [head_normalize(column_data[i])]

        new_col_data += [column_data[int(len(column_data) / 2)][:]]
        for i in range(int(len(column_data) / 2) + 1, len(column_data)):
            new_col_data += [head_normalize(column_data[i])]
        plot_helper(save_dir, "-head 100- " + title, column_names, new_col_data)
    else:
        new_col_data = [column_data[0][:]]
        for i in range(1, len(column_data)):
            new_col_data += [head_normalize(column_data[i])]
        plot_helper(save_dir, "-head 100- " + title, column_names, new_col_data)
        


def plot_heat_map(save_dir, title, column_names, column_data):
    if not os.path.exists(save_dir):
        print("creating dir", save_dir)
        os.makedirs(save_dir)

    level_to_points = {}
    for i in range(len(column_data[0])):
        qid = column_data[0][i]
        point = column_data[1][i]
        px, py = point.replace('"', '').split(",")
        px = float(px)
        py = float(py)
        if qid not in level_to_points:
            level_to_points[qid] = [[], []]
        level_to_points[qid][0].append(px)
        level_to_points[qid][1].append(py)
        

    COLORS = ["r", "g", "b", "c", "m", "y", "k", "lime"]
    C_IDX = 0
    # plot them points
    for qid in level_to_points:
        loc_x = level_to_points[qid][0]
        loc_y = level_to_points[qid][1]
        plt.scatter(loc_x, loc_y, c=COLORS[C_IDX], label=qid, marker='v', s=100, linewidths=0.1)
        C_IDX += 1

        # set a resonable bound
        bound_x_min = int(min(loc_x))
        bound_x_max = int(max(loc_x))
        bound_y_min = int(min(loc_y))
        bound_y_max = int(max(loc_y))
        if bound_y_max - bound_y_min < 4:
            bound_y_max += 6
        if bound_x_max - bound_x_min < 4:
            bound_x_max += 6

        plt.xticks(ticks=[i for i in range(bound_x_min, bound_x_max + 1)])
        plt.yticks(ticks=[i for i in range(bound_y_min, bound_y_max + 1)])

        plt.title(title + " level " + qid)
        plt.xlabel("X location")
        plt.ylabel("Y location")
        plt.legend(loc='best')

        save_path = save_dir + title + "lv" + qid + ".png"
        plt.savefig(save_path, transparent=True)
        plt.clf()


# one pass to populate data! ^
# title -> [[column names], [column data]]
def parse_data_file():
    data_lines = open("out/data.txt", "r").readlines()
    for i in range(len(data_lines)):
        data_lines[i] = data_lines[i].strip()

    queries = {}

    i = 1
    while i < len(data_lines):
        # 1 graph
        if data_lines[i] == "":
            i += 1
            continue
        # title
        title = data_lines[i]
        # print("Title", title)
        i += 1
        # columns
        column_names = data_lines[i].split("\t")
        # print("Columns", column_names)
        i += 1
        # begin graph
        graph_data = [[] for _ in column_names]
        while True:
            # grab a line
            if i >= len(data_lines) or data_lines[i] == "":
                # done
                break
            data_row = data_lines[i].split("\t")
            i += 1
            for col in range(len(graph_data)):
                graph_data[col].append(data_row[col])
        # parsing to float
        if "Locations" not in title:
            for arr in graph_data:
                for j in range(len(arr)):
                    arr[j] = float(arr[j])
        # sort 
        if len(graph_data[0]) >= 2 and graph_data[0][0] > graph_data[0][-1]:
            # reverse
            for column_data in graph_data:
                column_data.reverse()
                
        # save
        queries[title] = [column_names, graph_data]
    return queries


# read data.txt
ALL_GRAPH_DATA = parse_data_file()
# print(ALL_GRAPH_DATA.keys())
# print(ALL_GRAPH_DATA["Average Stats By Level (BOTH)"])
# exit()


# drawing
# # draw it
# A_graph_title = title.replace("(B)", "(A)")
# if A_graph_title in ALL_GRAPH_DATA:
# column_names = ALL_GRAPH_DATA[A_graph_title][0] + column_names
# graph_data = ALL_GRAPH_DATA[A_graph_title][1] + graph_data
# might be able to combine?
# if "(A)" in title:
#     # wait for B
#     continue
# elif "(B)" in title:
#     ALL_GRAPH_DATA[title] = [column_names, graph_data]
    # garph it!
    # print(graph_data)
OUT_GRAPH_DIR = "./out/graphs/"


for table_name in ALL_GRAPH_DATA:
    # garph it!
    if "Locations" in table_name:
        plot_heat_map(OUT_GRAPH_DIR, table_name, 
            ALL_GRAPH_DATA[table_name][0], ALL_GRAPH_DATA[table_name][1])
    else:
        if "Average Stats By Level" in table_name:
            if "(A)" in table_name:
                continue
            if "(BOTH)" in table_name:
                plot_and_save_graph_normal(OUT_GRAPH_DIR, table_name,
                    ALL_GRAPH_DATA[table_name][0], ALL_GRAPH_DATA[table_name][1])
                continue
            
            # this huge one split to multiple data
            a_table = table_name.replace("(B)", "(A)")
            if a_table not in ALL_GRAPH_DATA:
                print("E: cannot find", a_table)
                continue
            # split all columns 1 by 1
            for i in range(1, len(ALL_GRAPH_DATA[a_table][0])):
                a_col = [ALL_GRAPH_DATA[a_table][0][0], ALL_GRAPH_DATA[a_table][0][i]]
                a_data = [ALL_GRAPH_DATA[a_table][1][0], ALL_GRAPH_DATA[a_table][1][i]]
                b_col = [ALL_GRAPH_DATA[table_name][0][0], ALL_GRAPH_DATA[table_name][0][i]]
                b_data = [ALL_GRAPH_DATA[table_name][1][0], ALL_GRAPH_DATA[table_name][1][i]]
                plot_and_save_graph_normal(OUT_GRAPH_DIR,
                    "Z Average Stats By Level (AB) " + "(" + ALL_GRAPH_DATA[a_table][0][i] + ")",
                    a_col + b_col, a_data + b_data)


        elif "(A)" in table_name:
            # draw A
            plot_and_save_graph_normal(OUT_GRAPH_DIR, table_name,
                ALL_GRAPH_DATA[table_name][0], ALL_GRAPH_DATA[table_name][1])
        elif "(B)" in table_name:
            plot_and_save_graph_normal(OUT_GRAPH_DIR, table_name,
                ALL_GRAPH_DATA[table_name][0], ALL_GRAPH_DATA[table_name][1])
            # and plot A+B
            a_table = table_name.replace("(B)", "(A)")
            if a_table not in ALL_GRAPH_DATA:
                continue
            plot_and_save_graph_normal(OUT_GRAPH_DIR, table_name.replace("(B)", "(AB)"),
                ALL_GRAPH_DATA[a_table][0] + ALL_GRAPH_DATA[table_name][0],
                ALL_GRAPH_DATA[a_table][1] + ALL_GRAPH_DATA[table_name][1])
        else:
            # normal plot
            # normal
            # hack change to calculate rate
            col2 = ALL_GRAPH_DATA[table_name][1][1]
            copy_col2 = col2[:]
            for i in range(len(col2)):
                if i == 0:
                    col2[i] = 1
                else:
                    col2[i] = copy_col2[i] / copy_col2[i - 1]
            plot_and_save_graph_normal(OUT_GRAPH_DIR, table_name,
                ALL_GRAPH_DATA[table_name][0], ALL_GRAPH_DATA[table_name][1])
            