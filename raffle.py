import random
import json

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

def check_if_names_are_a_pair(x, y, pairs):
    """ Check if name x and y are a pairs """
    is_pair = False
    for k, v in pairs.items():
        if (x in v) and (y in v):
            is_pair = True
    return(is_pair)


def main():
    """ Main flow """
    pairs = read_pairs_files("pairs.json")
    results = []

    # Create the givers and receivers lists
    givers, receivers = all_names_to_list(pairs), all_names_to_list(pairs)

    # Shuffle the lists in place
    random.shuffle(givers), random.shuffle(receivers)


if __name__ == "__main__":
    main()