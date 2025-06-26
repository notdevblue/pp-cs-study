#include <stdio.h>
#include <string.h>

int main()
{
    int result = strcmp("blahblah", "blahblah");

    printf("result: %d\n", result);

    return 0;
}

/*
int strcmp(const char* s1, const char* s2)
{
    while(*s1 && (*s1 == *s2))
    {
        s1++;
        s2++;
    }
    return *(const unsigned char*)s1 - *(const unsigned char*)s2;
}
*/