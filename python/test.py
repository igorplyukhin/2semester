import re
import urllib.request


s = "a" * 30 + "b"*10
q = re.findall(r"\b(\w)\1{9}\b", s)
print(q)



urlib