def is_correct_password(s):
    lower=False
    upper=False
    digits=False
    if len(s) < 8:
        return False
    for ch in s:
        if (not (ch.isalpha() or ch.isdigit())):
            return False
        if (ch.isupper()):
            upper = True
        if (ch.islower()):
            lower=True
        if (ch.isdigit()):
            digits=True

    return lower and upper and digits

s=input()
if is_correct_password(s):
    print("YES")
else:
    print("NO")