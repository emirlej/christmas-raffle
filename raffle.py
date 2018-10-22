import os
import random
import json
import pandas as pd
from datetime import datetime


def read_pairs_files(fpath):
    with open(fpath, "r") as FILE:
        pairs = json.load(FILE)
    return(pairs)


def all_names_to_list(pairs):
    names = []
    for _, v in pairs.items():
        for name in v:
            names.append(name)
    return(names)


def check_if_names_are_pair(x, y, pairs):
    """ Check if name x and y are a pairs """
    is_pair = False
    for k, v in pairs.items():
        if (x in v) and (y in v):
            is_pair = True
    return(is_pair)


def get_length_of_two_lists(x, y):
    return(sum(list(map(len, [x, y]))))


def main():
    """ Main flow """
    pairs = read_pairs_files("pairs.json")
    tmp_results = []

    # Create the givers and receivers lists
    givers, receivers = all_names_to_list(pairs), all_names_to_list(pairs)
    # Shuffle the lists in place
    random.shuffle(givers), random.shuffle(receivers)

    n_restarts = 0

    while get_length_of_two_lists(givers, receivers) > 0:

        giver = random.choice(givers)
        receiver = random.choice(receivers)
        is_pair = check_if_names_are_pair(giver, receiver, pairs)

        # Try again if same pair, remove people from list if noe
        if is_pair:
            # If only two people are left and they are a pair
            if get_length_of_two_lists(givers, receivers) == 2:
                tmp_results.clear() # Clear the list
                givers, receivers = all_names_to_list(pairs), all_names_to_list(pairs)
                continue
            else:
                continue
        else:
            tmp_results.append((giver, receiver))
            givers.remove(giver)
            receivers.remove(receiver)

    # Convert into a dataframe
    results = pd.DataFrame(tmp_results, columns=["Giver", "Mottaker"])
    fname = "juletrekning_{}.csv".format(datetime.now().year)
    fpath = os.path.join("results", fname)

    # Create file only if it does not exist
    if not os.path.exists(fpath):
        results.to_csv(fpath, index=False)
    else:
        print("File '{}' exists".format(fpath))


if __name__ == "__main__":
    main()
