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


def main():
    """ Main flow """
    pairs = read_pairs_files("pairs.json")
    
    # Create the givers and creaters list
    givers = all_names_to_list(pairs)
    print(givers.copy())
    #receivers = all_names_to_list(pairs)

    print(givers)

if __name__ == "__main__":
    main()