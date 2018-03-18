exists($$PWD/{0}-default.pri) {{
    include($$PWD/{0}-default.pri)
}}

if(!debug_and_release|build_pass):CONFIG(debug, debug|release) {{
    include($$PWD/{0}-debug.pri)
}} else {{
    include($$PWD/{0}-release.pri)
}}